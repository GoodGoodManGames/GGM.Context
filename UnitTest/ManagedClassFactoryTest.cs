using GGMContext.Context.Factory;
using UnitTest.Dummy;
using Xunit;

namespace UnitTest
{
    public class ManagedClassFactoryTest
    {
        public ManagedClassFactoryTest()
        {
            _factory = new ManagedClassFactory(typeof(ManagedClassFactoryTest).Assembly);
        }

        private readonly ManagedClassFactory _factory;

        [Fact]
        public void FactoryProtoTest()
        {
            var deforeCount = ProtoManagedClass.ConstructorCount;
            var temp1 = _factory.GetManagedObject<ProtoManagedClass>();
            var temp2 = _factory.GetManagedObject<ProtoManagedClass>();
            Assert.True(temp1 != temp2);
            Assert.True(ProtoManagedClass.ConstructorCount == deforeCount + 2);
        }

        [Fact]
        public void FactorySingletonTest()
        {
            var deforeCount = SingletonManagedClass2.ConstructorCount;
            var temp1 = _factory.GetManagedObject<SingletonManagedClass2>();
            var temp2 = _factory.GetManagedObject<SingletonManagedClass2>();
            Assert.True(temp1 == temp2);
            Assert.True(SingletonManagedClass2.ConstructorCount == deforeCount);
        }
    }
}