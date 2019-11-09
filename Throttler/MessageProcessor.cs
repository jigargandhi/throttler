namespace Throttler
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Reactive;
    using System.Reactive.Subjects;
    using System.Reactive.Linq;
    using System.Reactive.Concurrency;

    public class MessageProcessor
    {
        private readonly IApiClient client;
        private readonly int notificationRate;

        private readonly TimeSpan timeSpan;
        private IScheduler scheduler;
        public MessageProcessor(IApiClient client, int notificationRate) : this(client, notificationRate, ThreadPoolScheduler.Instance)
        {

        }

        public MessageProcessor(IApiClient client, int notificationRate, IScheduler scheduler)//: this(client, notificationRate)
        {
            this.client = client;
            this.notificationRate = notificationRate; // per second
            timeSpan = TimeSpan.FromMilliseconds(1000.0 / notificationRate);
            this.scheduler = scheduler;
        }
        public async Task SendNotification(int userId)
        {
            await client.PostAsync(userId);
        }
        public void Publish(List<int> userIds)
        {
            var subject = new Subject<int>();
            subject
                // Zip takes an combines one from both the subject and interval 
                // and picks up the new as per the zip function
                .Zip<int, long, int>(Observable.Interval(timeSpan, scheduler), (int x, long y) => x)
                .Select(userId => Observable.FromAsync(() => SendNotification(userId), scheduler))
                .Concat()
                .Subscribe();
            userIds.ForEach(userId => subject.OnNext(userId));
            subject.OnCompleted();
        }

    }
}