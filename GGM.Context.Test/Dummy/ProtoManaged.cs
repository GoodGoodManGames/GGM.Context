using GGM.Context;
using GGM.Context.Attribute;

namespace UnitTest.Dummy
{
    [Managed(ManagedType.Proto)]
    public class ProtoManaged
    {
        public ProtoManaged()
        {
        }
        
        public ProtoManaged(int a)
        {
        }
        [AutoWired]
        public ProtoManaged(ProtoManaged_1 managed)
        {
        }
        
        public ProtoManaged(object managed)
        {
        }
    }

    [Managed(ManagedType.Proto)]
    public class ProtoManaged_1
    {
        public ProtoManaged_1()
        {
        }
    }
}
