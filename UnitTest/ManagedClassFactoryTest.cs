using System;
using GGMContext.Context;
using GGMContext.Context.Attribute;
using GGMContext.Context.Factory;
using Xunit;

namespace UnitTest
{
    public class ManagedClassFactoryTest
    {
        private ManagedClassFactory _factory;
        public ManagedClassFactoryTest()
        {
            _factory = new ManagedClassFactory(typeof(ManagedClassFactoryTest).Assembly);
        }

        [Fact]
        public void FactorySingletonTest()
        {
            int deforeCount = SingletonManagedClass2.ConstructorCount;
            var temp1 = _factory.GetManagedObject<SingletonManagedClass2>();
            var temp2 = _factory.GetManagedObject<SingletonManagedClass2>();
            Assert.True(temp1 == temp2);
            Assert.True(SingletonManagedClass2.ConstructorCount == deforeCount);
        }
        
        [Fact]
        public void FactoryProtoTest()
        {
            int deforeCount = ProtoManagedClass.ConstructorCount;
            var temp1 = _factory.GetManagedObject<ProtoManagedClass>();
            var temp2 = _factory.GetManagedObject<ProtoManagedClass>();
            Assert.True(temp1 != temp2);
            Assert.True(ProtoManagedClass.ConstructorCount == deforeCount + 2);
        }
    }

    [Managed(ManagedClassType.Singleton)]
    public class SingletonManagedClass1
    {
        public SingletonManagedClass1()
        {
            Console.WriteLine($"{nameof(SingletonManagedClass1)} is Created");
        }
    }
    
    [Managed(ManagedClassType.Singleton)]
    public class SingletonManagedClass2
    {
        public static int ConstructorCount = 0; 
        public SingletonManagedClass2()
        {
            Console.WriteLine($"{nameof(SingletonManagedClass2)} is Created");
            ConstructorCount++;
        }
        
        [AutoWired]
        public SingletonManagedClass2(SingletonManagedClass1 smc1) : this()
        {
        }
    }

    [Managed(ManagedClassType.Proto)]
    public class ProtoManagedClass
    {
        public static int ConstructorCount = 0; 
        public ProtoManagedClass()
        {            
            Console.WriteLine($"{nameof(ProtoManagedClass)} is Created");
            ConstructorCount++;
        }
        
        [AutoWired]
        public ProtoManagedClass(SingletonManagedClass2 smc2) : this()
        {
        }
    }
}