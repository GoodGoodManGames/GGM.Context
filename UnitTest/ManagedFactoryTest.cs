using GGM.Context.Factory;
using UnitTest.Dummy;
using Xunit;

namespace UnitTest
{
    public class ManagedFactoryTest
    {
        public ManagedFactoryTest()
        {
            _factory = new ManagedFactory();
        }

        private readonly ManagedFactory _factory;

        [Fact]
        public void DefaultConstructorFactoryTest()
        {
            var defaultConstructorObject = _factory.Create<DefaultConstructorClass>();
            Assert.NotNull(defaultConstructorObject);
        }

        [Fact]
        public void ConstructorFactoryTest()
        {
            var constructorObject = _factory.Create<ConstructorClass>(new object[] { 3, "asd" });
            Assert.NotNull(constructorObject);
        }
    }
}