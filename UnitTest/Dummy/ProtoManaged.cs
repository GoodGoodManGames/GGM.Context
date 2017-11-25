using GGM.Context;
using GGM.Context.Attribute;

namespace UnitTest.Dummy
{
    [Managed(ManagedType.Proto)]
    public class ProtoManaged
    {
        [AutoWired]
        public ProtoManaged(ProtoManaged_1 managed) => ManagedContextTest.Output.WriteLine(nameof(ProtoManaged));
    }

    [Managed(ManagedType.Proto)]
    public class ProtoManaged_1
    {
        public ProtoManaged_1() => ManagedContextTest.Output.WriteLine(nameof(ProtoManaged_1));
    }
}
