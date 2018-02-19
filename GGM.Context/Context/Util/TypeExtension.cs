using System;
using System.Linq;
using System.Reflection;

namespace GGM.Context.Util
{
    internal static class TypeExtension
    {
        public static Type[] GetParameterTypes(this MethodBase self)
        {
            if (self == null)
                return Type.EmptyTypes;
            var parameters = self.GetParameters();

            return parameters
                       ?.Select(info => info.ParameterType)
                       ?.ToArray() ?? Type.EmptyTypes;
        }
    }
}