using GGMContext.Context.Attribute;
using System;
using System.Collections.Generic;
using System.Text;

namespace GGMContext.Context
{
    internal class ManagedInfo
    {
        internal ManagedInfo(Type type, ManagedAttribute managedAttribute, Func<object> generator)
        {
            Type = type;
            ManagedAttribute = managedAttribute;
            mGenerator = generator;
        }

        public Type Type { get; }
        public ManagedAttribute ManagedAttribute { get; }
        Func<object> mGenerator { get; }
        public object Object => mGenerator();

        public ManagedType ManagedType => ManagedAttribute.ManagedType;
    }
}
