using GGMContext.Context;
using GGMContext.Context.Attribute;
using System;
using System.Collections.Generic;
using System.Text;

namespace UnitTest.Dummy
{
    [Managed(ManagedType.Singleton)]
    public class SingletonManaged
    {
        [AutoWired]
        public SingletonManaged(ProtoManaged protoManaged, ProtoManaged_1 protoManaged_1) => ManagedContextTest.Output.WriteLine(nameof(SingletonManaged));
    }

    [Managed(ManagedType.Singleton)]
    public class SingletonManaged_1
    {
        [AutoWired]
        public SingletonManaged_1(SingletonManaged singleton) => ManagedContextTest.Output.WriteLine(nameof(SingletonManaged_1));
    }
}
