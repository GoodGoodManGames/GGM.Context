using GGM.Context;
using GGM.Context.Attribute;

namespace UnitTest.Dummy
{
    [Configuration]
    public class TestConfiguration
    {
        public const string Name = "GGM";
        public const int Age = 33;

        [Managed(ManagedType.Singleton)]
        public Test1 GetTest1() { return new Test1(); }

        [Managed(ManagedType.Proto)]
        public Person GetPerson() { return new Person {Name = Name, Age = Age}; }
    }

    public class Test1
    {
    }

    public class Person
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }
}