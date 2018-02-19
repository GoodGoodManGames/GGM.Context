using System;

namespace GGM.Context
{
    public class ManagedInfo
    {
        public ManagedInfo(Type type, Func<object> generator)
        {
            Type = type;
            mGenerator = generator;
        }

        public Type Type { get; }
        Func<object> mGenerator { get; }
        public object Object => mGenerator();
    }
}