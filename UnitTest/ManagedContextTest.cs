using GGM.Context;
using UnitTest.Dummy;
using Xunit;
using Xunit.Abstractions;

namespace UnitTest
{
    public class ManagedContextTest
    {
        public ManagedContextTest(ITestOutputHelper output)
        {
            _context = new ManagedContext();
            _context.Register(typeof(ProtoManaged));
            _context.Register(typeof(ProtoManaged_1));
            _context.Register(typeof(SingletonManaged));
            _context.Register(typeof(SingletonManaged_1));
            _context.Register(typeof(TestConfiguration));
        }

        private readonly ManagedContext _context;

        [Fact]
        public void CreateProtoManaged()
        {
            var protoManaged = _context.GetManaged<ProtoManaged>();
            Assert.NotNull(protoManaged);
        }

        [Fact]
        public void CreateSingletonManaged()
        {
            var singletonManaged = _context.GetManaged<SingletonManaged>();
            Assert.NotNull(singletonManaged);
        }

        [Fact]
        public void CheckProtoReferenceEqual()
        {
            var protoManaged_0 = _context.GetManaged<ProtoManaged>();
            var protoManaged_1 = _context.GetManaged<ProtoManaged>();
            Assert.NotEqual(protoManaged_0, protoManaged_1);
        }

        [Fact]
        public void CheckSingletonReferenceEqual()
        {
            var singletonManaged_0 = _context.GetManaged<SingletonManaged>();
            var singletonManaged_1 = _context.GetManaged<SingletonManaged>();
            Assert.Equal(singletonManaged_0, singletonManaged_1);
        }

        [Fact]
        public void CheckAutoWired()
        {
            var singletonManaged = _context.GetManaged<SingletonManaged>();
            Assert.NotNull(singletonManaged.ProtoManaged0);
            Assert.NotNull(singletonManaged.ProtoManaged1);
            Assert.NotNull(singletonManaged.SingletonManaged1);
            Assert.NotEqual(singletonManaged.ProtoManaged0, singletonManaged.ProtoManaged1);
            Assert.Equal(singletonManaged.SingletonManaged0, singletonManaged.SingletonManaged1);
        }

        [Fact]
        public void CheckConfigurationProto()
        {
            var person = _context.GetManaged<Person>();
            Assert.Equal(TestConfiguration.Name, person.Name);
            Assert.Equal(TestConfiguration.Age, person.Age);
            var secondPerson = _context.GetManaged<Person>();
            Assert.NotEqual(person, secondPerson);
        }

        [Fact]
        public void CheckConfigurationSingleton()
        {
            var test1 = _context.GetManaged<Test1>();
            var Test1_1 = _context.GetManaged<Test1>();
            Assert.Equal(test1, Test1_1);
        }
    }
}