using Xunit;
using System;
using System.Threading;
using System.Threading.Tasks;


namespace ServerTests
{
    public class UnitTestServerStat
    {
        [Fact]
        public void TestGetCount_ShouldReturnCorrectInitialCount()
        { 
            var actualCount = Server.GetCount();
            Assert.Equal(0, actualCount);
        }

        [Fact]
        public void TestAddToCount_ShouldIncrementCounter()
        {
            var initialCount = Server.GetCount();
            var incrementAmount = 10;

            Server.AddToCount(incrementAmount);
            var updatedCount = Server.GetCount();

            Assert.Equal(initialCount + incrementAmount, updatedCount);
        }

        [Fact]
        public void TestAddToCount_MultipleValues_ShouldSumTheValues()
        {
            var initialCount = Server.GetCount();
            var values = new List<int> { 5, 3 };

            foreach (var value in values)
            {
                Server.AddToCount(value);
            }
            var finalCount = Server.GetCount();

            Assert.Equal(initialCount + values.Sum(), finalCount);
        }

        [Fact]
        public void TestAddToCount_Async_ShouldIncrementCounter()
        {
            var initialCount = Server.GetCount();
            var incrementAmount = 5;

            var task = Task.Run(() => Server.AddToCount(incrementAmount));
            task.Wait();
            var finalCount = Server.GetCount();

            Assert.Equal(initialCount + incrementAmount, finalCount);
        }
    }
}