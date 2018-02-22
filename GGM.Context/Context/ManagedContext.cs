using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;


namespace GGM.Context
{
    using Attribute;
    using Exception;

    public class ManagedContext
    {
        private readonly List<BaseManagedDefinition> _definitions = new List<BaseManagedDefinition>(128);
        private readonly Dictionary<Type, Func<object>> _managedGetters = new Dictionary<Type, Func<object>>();
        private readonly HashSet<Assembly> _registeredAssemblies = new HashSet<Assembly>();

        

        /// <summary>
        /// Assembly를 Context에 등록합니다, Context는 해당 Assembly의 모든 타입 중 ManagedAttribute가 걸린 클래스들을 ManagedDefinition으로 만들어 저장합니다.
        /// </summary>
        /// <param name="assembly">등록할 Assembly</param>
        public virtual void Register(Assembly assembly)
        {
            if(assembly == null)
                throw new ArgumentNullException(nameof(assembly));
            
            _registeredAssemblies.Add(assembly);
            var types = assembly.GetTypes().Where(item => item.IsDefined(typeof(ManagedAttribute)));
            foreach (var type in types)
                Register(type);
        }

        #region void Register(Type registerType)

        private void RemoveDefinitionIfExist(Type targetType)
        {
            bool IsRegisted(BaseManagedDefinition definition) => definition.TargetType == targetType;

            if (!_definitions.Exists(IsRegisted)) 
                return;
            
            // 이미 등록된 타입이면 지움
            _definitions.RemoveAll(IsRegisted);
        }
        
        private void Register(Type registerType)
        {
            RemoveDefinitionIfExist(registerType);

            // ConstructorDefinition 생성
            var constructorInfo = registerType.GetConstructors().FirstOrDefault(info => info.IsDefined(typeof(AutoWiredAttribute)));
            if (constructorInfo == null)
                constructorInfo = registerType.GetConstructor(Type.EmptyTypes);

            var constructorDefinition = new ConstructorManagedDefinition(constructorInfo);
            _definitions.Add(constructorDefinition);

            // 해당 타입이 Configuration 클래스인 경우면 메소드들을 등록해준다
            if (registerType.IsDefined(typeof(ConfigurationAttribute)))
            {
                var managedMethodInfo = registerType.GetMethods().Where(info => info.IsDefined(typeof(ManagedAttribute)));
                foreach (var methodInfo in managedMethodInfo)
                {
                    RemoveDefinitionIfExist(methodInfo.ReturnType);

                    var configurationDefinition = new ConfigurationManagedDefinition(methodInfo);
                    _definitions.Add(configurationDefinition);
                }
            }
        }

        #endregion Register

        /// <summary>
        /// 객체를 질의합니다.
        /// </summary>
        /// <typeparam name="T">질의할 클래스타입</typeparam>
        /// <returns>질의된 객체</returns>
        public virtual T GetManaged<T>() where T : class => GetManaged(typeof(T)) as T;

        /// <summary>
        /// 객체를 질의합니다.
        /// </summary>
        /// <param name="type">질의할 클래스타입</param>
        /// <returns>질의된 객체</returns>
        public virtual object GetManaged(Type type)
        {
            if(type == null)
                throw new ArgumentNullException(nameof(type));
            
            // 아직 등록되지 않은 Assembly면 등록
            if (!_registeredAssemblies.Contains(type.Assembly))
                Register(type.Assembly);

            // 이미 만들어둔 getter가 있으면 새로 만들지 않음.
            if (_managedGetters.TryGetValue(type, out var cachedManagedGetter))
                return cachedManagedGetter();

            
            var definition = _definitions.FirstOrDefault(info => info.TargetType == type);
            CreateManagedException.Check(definition != null, CreateManagedError.NotManagedClass);

            var parameters = definition.NeedParameterTypes.Select(GetManaged).ToArray();
            Func<object> managedGetter = CreateGetter(definition, parameters);
            _managedGetters[type] = managedGetter;
            return managedGetter();
        }

        /// <summary>
        /// definition을 이용하여 객체를 생성합니다. 
        /// </summary>
        /// <param name="definition">사용할 definition</param>
        /// <param name="parameters">definition이 사용할 인자</param>
        /// <returns>definition이 생성한 객체</returns>
        public virtual object Create(BaseManagedDefinition definition, object[] parameters)
        {
            if(definition == null)
                throw new ArgumentNullException(nameof(definition));
            if(parameters == null)
                throw new ArgumentNullException(nameof(parameters));
            
            return definition.Generate(parameters); 
        }

        private Func<object> CreateGetter(BaseManagedDefinition definition, object[] parameters)
        {
            Func<object> managedGetter;
            switch (definition.ManagedType)
            {
                case ManagedType.Singleton:
                    // 싱글턴인 경우 객체를 만든 뒤 캡쳐
                    var managed = Create(definition, parameters);
                    managedGetter = () => managed;
                    break;
                case ManagedType.Proto:
                    // 프로토인 경우 객체를 만드는 것을 캡쳐
                    managedGetter = () => Create(definition, parameters);
                    break;
                default: 
                    throw new CreateManagedException(CreateManagedError.UnsupportedManagedType);
            }

            return managedGetter;
        }
    }
}