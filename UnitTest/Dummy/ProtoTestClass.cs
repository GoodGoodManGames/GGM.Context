using GGMContext.Context;
using GGMContext.Context.Attribute;

namespace UnitTest.Dummy
{
    [Managed(ManagedClassType.Proto)]
    public class ProtoManagedClass
    {
        public static int ConstructorCount;

        public ProtoManagedClass()
        {
            ConstructorCount++;
        }

        [AutoWired]
        public ProtoManagedClass(SingletonManagedClass2 smc2) : this()
        {
        }
    }
}