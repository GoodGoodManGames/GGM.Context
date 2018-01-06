using GGM.Context;
using GGM.Context.Attribute;

namespace UnitTest.Dummy
{
    [Managed(ManagedType.Singleton)]
    public class SingletonManaged
    {
        public ProtoManaged ProtoManaged0 { get; }
        public ProtoManaged ProtoManaged1 { get; }
        public SingletonManaged_1 SingletonManaged0 { get; }
        public SingletonManaged_1 SingletonManaged1 { get; }

        [AutoWired]
        public SingletonManaged(ProtoManaged protoManaged0, ProtoManaged protoManaged1
            , SingletonManaged_1 singletonManaged0, SingletonManaged_1 singletonManaged1)
        {
            ProtoManaged0 = protoManaged0;
            ProtoManaged1 = protoManaged1;
            SingletonManaged0 = singletonManaged0;
            SingletonManaged1 = singletonManaged1;
        }
    }

    [Managed(ManagedType.Singleton)]
    public class SingletonManaged_1
    {
    }
}
