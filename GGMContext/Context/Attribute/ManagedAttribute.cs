using System;

namespace GGMContext.Context.Attribute
{
    /// <summary>
    ///     클래스가 ManagedClass임을 지정합니다.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class ManagedAttribute : System.Attribute
    {
        public ManagedAttribute(ManagedClassType managedClassType)
        {
            ClassType = managedClassType;
        }
     
        /// <summary>
        ///     생명주기 타입을 지정합니다.
        /// </summary>
        public ManagedClassType ClassType { get; private set; }
    }
}