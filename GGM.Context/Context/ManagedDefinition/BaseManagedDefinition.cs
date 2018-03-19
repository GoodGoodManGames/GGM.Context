using System;
using System.Reflection;

namespace GGM.Context
{
    using Attribute;
    using Exception;

    // TODO: 추후 구조가 좀 더 정리되면 internal로 변경하는 방향으로 나갈 예정.
    /// <summary>
    /// Managed 생성에 대한 정보를 가지고 있는 클래스입니다.
    /// </summary>
    public abstract class BaseManagedDefinition
    {
        /// <summary>
        /// 객체를 생성하는 Delegate입니다.
        /// </summary>
        /// <param name="parameters">객체 생성 인자</param>
        protected delegate object Generator(params object[] parameters);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="targetType">Managed타입</param>
        protected BaseManagedDefinition(Type targetType)
        {
            TargetType = targetType;
            GeneratorName = $"{TargetType.Name}Factory+{Guid.NewGuid()}";
        }
        
        /// <summary>
        /// Definition이 가지고 있는 정보의 타입 
        /// </summary>
        public Type TargetType { get; }

        /// <summary>
        /// 객체 생명주기 타입입니다.
        /// </summary>
        public abstract ManagedType ManagedType { get; }
        
        /// <summary>
        /// 객체 생성에 필요한 인자 타입 리스트입니다.
        /// </summary>
        public abstract Type[] NeedParameterTypes { get; }
        
        /// <summary>
        /// 객체를 생성하는 Delegate입니다.
        /// </summary>
        protected abstract Generator ManagedGenerator { get; }
        
        protected string GeneratorName { get; }
        
        /// <summary>
        /// 등록된 클래스를 객채로 생성합니다.
        /// </summary>
        /// <param name="parameters">객체 생성에 필요한 인자</param>
        /// <returns>생성된 객체</returns>
        internal object Generate(object[] parameters)
        {
            if(parameters == null)
                throw new ArgumentNullException(nameof(parameters));
            
            return ManagedGenerator(parameters);
        }
    }
}