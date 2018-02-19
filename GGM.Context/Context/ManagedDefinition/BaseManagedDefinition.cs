using System;

namespace GGM.Context
{
    /// <summary>
    /// Managed 생성에 대한 정보를 가지고 있는 클래스입니다.
    /// Configuration으로 등록된 경우엔 configuration을 가집니다.
    /// </summary>
    public abstract class BaseManagedDefinition
    {
        protected delegate object Generator(object[] parameters);
        
        public BaseManagedDefinition(Type targetType, Type[] parameterTypes)
        {
            TargetType = targetType;
            ParameterTypes = parameterTypes;
        }
        
        public Type TargetType { get; }
        public Type[] ParameterTypes { get; }

        public abstract ManagedType ManagedType { get; }
        public abstract Type[] NeedParameterTypes { get; } 
        protected abstract Generator ManagedGenerator { get; }
        
        public object Generate(object[] parameters)
        {
            if (ManagedGenerator == null)
                throw new System.Exception("Generator가 생성되지 않았습니다.");
            return ManagedGenerator(parameters);
        }
    }
}