using GGM.Context.Factory;
using UnitTest.Dummy;
using Xunit;

namespace UnitTest
{
    public class ManagedFactoryTest
    {
        public ManagedFactoryTest()
        {
            mFactory = new ManagedFactory();
        }

        private readonly ManagedFactory mFactory;

        [Fact]
        public void DefaultConstructorFactoryTest()
        {
            var defaultConstructorObject = mFactory.Create<DefaultConstructorClass>();
            Assert.NotNull(defaultConstructorObject);
        }

        [Fact]
        public void ConstructorFactoryTest()
        {
            var constructorObject = mFactory.Create<ConstructorClass>(new object[] { 3, "asd" });
            Assert.NotNull(constructorObject);
        }
    }
}