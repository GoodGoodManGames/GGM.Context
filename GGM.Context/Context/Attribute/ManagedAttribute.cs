using System;

namespace GGM.Context.Attribute
{
    /// <summary>
    ///     클래스가 ManagedClass임을 지정합니다.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class ManagedAttribute : System.Attribute
    {
        public ManagedAttribute(ManagedType managedType)
        {
            ManagedType = managedType;
        }
     
        /// <summary>
        ///     생명주기 타입을 지정합니다.
        /// </summary>
        public ManagedType ManagedType { get; private set; }
    }
}