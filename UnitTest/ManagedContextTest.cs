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
            Output = output;
            mContext = new ManagedContext();
        }

        public static ITestOutputHelper Output;
        private readonly ManagedContext mContext;

        [Fact]
        public void ManagedContextGetObjectTest()
        {
            var managed = mContext.GetManaged<SingletonManaged_1>();
            var managed1 = mContext.GetManaged<SingletonManaged_1>();
            var managed2 = mContext.GetManaged<SingletonManaged_1>();
            Assert.NotNull(managed);
            Assert.Equal(managed, managed1);
            Assert.Equal(managed1, managed2);
        }
    }
}
