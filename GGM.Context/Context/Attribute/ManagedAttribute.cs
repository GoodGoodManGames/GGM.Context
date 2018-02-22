using System;

namespace GGM.Context.Attribute
{
    /// <summary>
    /// 클래스가 ManagedClass임을 지정합니다.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class ManagedAttribute : System.Attribute
    {
        public ManagedAttribute(ManagedType managedType) { ManagedType = managedType; }

        /// <summary>
        /// 생명주기 타입.
        /// </summary>
        public ManagedType ManagedType { get; private set; }
    }
}