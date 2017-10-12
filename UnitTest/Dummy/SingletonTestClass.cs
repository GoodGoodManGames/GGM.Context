using GGMContext.Context;
using GGMContext.Context.Attribute;

namespace UnitTest.Dummy
{
    [Managed(ManagedClassType.Singleton)]
    public class SingletonManagedClass1
    {
    }

    [Managed(ManagedClassType.Singleton)]
    public class SingletonManagedClass2
    {
        public static int ConstructorCount;

        public SingletonManagedClass2()
        {
            ConstructorCount++;
        }

        [AutoWired]
        public SingletonManagedClass2(SingletonManagedClass1 smc1) : this()
        {
        }
    }
}