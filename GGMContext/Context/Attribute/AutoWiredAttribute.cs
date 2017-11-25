using System;

namespace GGM.Context.Attribute
{
    /// <summary>
    ///     생성자가 의존성을 주입받을 것을 지정합니다.
    /// </summary>
    [AttributeUsage(AttributeTargets.Constructor)]
    public class AutoWiredAttribute : System.Attribute
    {
    }
}