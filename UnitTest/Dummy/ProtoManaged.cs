using GGMContext.Context;
using GGMContext.Context.Attribute;
using System;
using System.Collections.Generic;
using System.Text;

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
