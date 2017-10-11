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
        
        public ManagedClassType ClassType { get; private set; }
    }
}