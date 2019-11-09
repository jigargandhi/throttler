namespace Throttler.Test
{
    using Microsoft.Reactive.Testing;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Xunit;


    public class MessageProcessorTest
    {
        [Fact]
        public void PublishTest()
        {
            // Arrange
            var scheduler = new TestScheduler();
            var testUserIds = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            MockApiClient client = new MockApiClient();
            var ticks = TimeSpan.FromMilliseconds(500).Ticks;

            // Act
            MessageProcessor messageProcessor = new MessageProcessor(client, 2, scheduler);
            messageProcessor.Publish(testUserIds);

            // Assert
            for (int i = 0; i < testUserIds.Count; i++)
            {
                scheduler.AdvanceBy(ticks);
                Assert.Equal(testUserIds[i], client.Value);
            }
        }

        public class MockApiClient : IApiClient
        {
            public int Value = 0;
            public async Task<bool> PostAsync(int userId)
            {
                Value = userId;
                return await Task.FromResult(true);
            }
        }
    }
}
