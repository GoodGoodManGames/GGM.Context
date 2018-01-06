using GGM.Context.Attribute;
using GGM.Context.Exception;
using GGM.Context.Factory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace GGM.Context
{
    /// <summary>
    ///     Managed가 태그된 객체들을 보관하고, 질의하는 클래스입니다.
    /// </summary>
    public class ManagedContext : ManagedFactory
    {
        private readonly Dictionary<Type, ManagedInfo> _managedInfos = new Dictionary<Type, ManagedInfo>();

        /// <summary>
        ///     객체를 질의합니다.
        /// </summary>
        /// <typeparam name="T">질의할 클래스타입</typeparam>
        /// <returns>질의된 객체</returns>
        public virtual T GetManaged<T>() where T : class => GetManaged(typeof(T)) as T;

        /// <summary>
        ///     객체를 질의합니다.
        /// </summary>
        /// <param name="type">질의할 클래스타입</param>
        /// <returns>질의된 객체</returns>
        public virtual object GetManaged(Type type)
        {
            if (_managedInfos.TryGetValue(type, out var cachedManagedInfo))
                return cachedManagedInfo.Object;

            var managedAttribute = type.GetCustomAttribute<ManagedAttribute>();
            CreateManagedException.Check(managedAttribute != null, CreateManagedError.NotManagedClass);
            
            var parameterInfos = GetInjectedParameters(type);
            var parameters = parameterInfos.Select(info => GetManaged(info.ParameterType)).ToArray();

            ManagedInfo managedInfo;
            var managedType = managedAttribute.ManagedType;
            switch (managedType)
            {
                case ManagedType.Singleton:
                    var managed = Create(type, parameters);
                    managedInfo = new ManagedInfo(type, managedAttribute, () => managed);
                    break;
                case ManagedType.Proto:
                    managedInfo = new ManagedInfo(type, managedAttribute, () => Create(type, parameters));
                    break;
                default: throw new CreateManagedException(CreateManagedError.UnsupportedManagedType);
            }

            _managedInfos[type] = managedInfo;
            return managedInfo.Object;
        }

        /// <summary>
        ///     타입에서 Injected될 인자들의 정보들을 가져옵니다.
        /// </summary>
        /// <param name="type">인자들의 정보를 가져올 대상 타입</param>
        /// <returns>대상 타입에 주입될 인자들의 정보</returns>
        protected virtual ParameterInfo[] GetInjectedParameters(Type type)
        {
            var constructorInfo = type.GetConstructors().FirstOrDefault(info => info.IsDefined(typeof(AutoWiredAttribute)));
            if (constructorInfo == null)
                Console.WriteLine($"{type}의 AutoWired 생성자가 없어 기본 생성자를 사용합니다.");
            return constructorInfo?.GetParameters() ?? new ParameterInfo[] { };
        }
    }
}
