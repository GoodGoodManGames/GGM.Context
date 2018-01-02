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
        private Dictionary<Type, ManagedInfo> mManagedInfos = new Dictionary<Type, ManagedInfo>();

        public virtual T GetManaged<T>() where T : class => GetManaged(typeof(T)) as T;

        public virtual object GetManaged(Type type)
        {
            if (mManagedInfos.TryGetValue(type, out ManagedInfo cachedManagedInfo))
                return cachedManagedInfo.Object;

            var managedAttribute = type.GetCustomAttribute<ManagedAttribute>();
            if (managedAttribute == null) throw new CreateManagedException(CreateManagedError.NotManagedClass);

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

            mManagedInfos[type] = managedInfo;
            return managedInfo.Object;
        }

        protected virtual ParameterInfo[] GetInjectedParameters(Type type)
        {
            var constructorInfo = type.GetConstructors().FirstOrDefault(info => info.IsDefined(typeof(AutoWiredAttribute)));
            if (constructorInfo == null)
                Console.WriteLine($"{type}의 AutoWired 생성자가 없어 기본 생성자를 사용합니다.");
            return constructorInfo?.GetParameters() ?? new ParameterInfo[] { };
        }
    }
}
