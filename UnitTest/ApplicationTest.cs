using System;
using GGMContext;
using GGMContext.Context;
using Xunit;

namespace UnitTest
{
    public class ApplicationTest
    {
        [Fact]
        public void RunApplicationTest()
        {
            ApplicationContext context = null;
            try
            {
                context = GGMApplication.Run(typeof(ApplicationTest), null);
            }
            catch (ArgumentException e)
            {
                Assert.True(true);
                Console.WriteLine(e);
                throw;
            }
            catch (NullReferenceException e)
            {
                Assert.True(true);
                Console.WriteLine(e);
            }
            catch (Exception e)
            {
                Assert.True(true);
                Console.WriteLine(e);
            }

            Assert.NotNull(context.GetManagedObject<Dummy.ProtoManagedClass>());
            Assert.NotNull(context.GetManagedObject<Dummy.SingletonManagedClass1>());
            Assert.NotNull(context.GetManagedObject<Dummy.SingletonManagedClass1>());
        }
    }
}