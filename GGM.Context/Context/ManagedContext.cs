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
        private List<BaseManagedDefinition> _managedDefinitions = new List<BaseManagedDefinition>(128);
        private readonly Dictionary<Type, ManagedInfo> _managedInfos = new Dictionary<Type, ManagedInfo>();

        public virtual void Register(Type registerType)
        {
            RemoveDefinitionIfExist(registerType);

            // ConstructorDefinition 생성
            var constructorInfo = registerType.GetConstructors().FirstOrDefault(info => info.IsDefined(typeof(AutoWiredAttribute)));
            if (constructorInfo == null)
            {
                Console.WriteLine($"{registerType}의 AutoWired 생성자가 없습니다, 이는 기본 생성자를 사용합니다.");
                constructorInfo = registerType.GetConstructor(Type.EmptyTypes);
            }

            var constructorDefinition = new ConstructorManagedDefinition(registerType, constructorInfo);
            _managedDefinitions.Add(constructorDefinition);

            // 해당 타입이 Configuration 클래스인 경우면 메소드들을 등록해준다
            if (registerType.IsDefined(typeof(ConfigurationAttribute)))
            {
                var managedMethodInfo = registerType.GetMethods().Where(info => info.IsDefined(typeof(ManagedAttribute)));
                foreach (var methodInfo in managedMethodInfo)
                {
                    RemoveDefinitionIfExist(methodInfo.ReturnType);

                    var configurationDefinition = new ConfigurationManagedDefinition(registerType, methodInfo);
                    _managedDefinitions.Add(configurationDefinition);
                }
            }
        }

        private void RemoveDefinitionIfExist(Type targetType)
        {
            bool IsRegisted(BaseManagedDefinition definition) => definition.TargetType == targetType;

            // 이미 등록된 타입이면 지움
            if (_managedDefinitions.Exists(IsRegisted))
            {
                Console.WriteLine($"{targetType.FullName}은 이미 Register된 타입입니다. 나중에 지정된 타입을 사용합니다.");
                _managedDefinitions.RemoveAll(IsRegisted);
            }
        }

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
            if (_managedInfos.TryGetValue(type, out var cachedManagedInfo))
                return cachedManagedInfo.Object;

            var definition = _managedDefinitions.FirstOrDefault(info => info.TargetType == type);
            CreateManagedException.Check(definition != null, CreateManagedError.NotManagedClass);

            var parameters = definition.NeedParameterTypes.Select(GetManaged).ToArray();
            ManagedInfo managedInfo = CreateManagedInfo(definition, parameters);
            _managedInfos[type] = managedInfo;
            return managedInfo.Object;
        }

        public virtual object Create(BaseManagedDefinition definition, object[] parameters) => definition.Generate(parameters);

        private ManagedInfo CreateManagedInfo(BaseManagedDefinition definition, object[] parameters)
        {
            ManagedInfo managedInfo;
            switch (definition.ManagedType)
            {
                case ManagedType.Singleton:
                    var managed = Create(definition, parameters);
                    managedInfo = new ManagedInfo(definition.TargetType, () => managed);
                    break;
                case ManagedType.Proto:
                    managedInfo = new ManagedInfo(definition.TargetType, () => Create(definition, parameters));
                    break;
                default: throw new CreateManagedException(CreateManagedError.UnsupportedManagedType);
            }

            return managedInfo;
        }
    }
}