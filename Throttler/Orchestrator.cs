namespace Throttler
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Messaging;
    using System.Text;
    using System.Threading.Tasks;
    using System.Timers;

    public class Orchestrator
    {
        IApiClient client;
        MessageQueue messageQueue;
        MessageProcessor messageProcessor;
        Timer timer;

        public Orchestrator(IApiClient client, string queuePath, int notificateRate)
        {
            this.client = client;
            messageQueue = new MessageQueue(queuePath);
            messageProcessor = new MessageProcessor(client, notificateRate);
            timer = new Timer(60 * 1000);
            timer.Elapsed += Timer_Elapsed;
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                Message message = messageQueue.Receive(TimeSpan.FromSeconds(2), MessageQueueTransactionType.None);
                XmlMessageFormatter formatter = new XmlMessageFormatter(new Type[] { typeof(string) });
                var intArray = formatter.Read(message);
                messageProcessor.Publish(JsonConvert.DeserializeObject<List<int>>(intArray.ToString()));
            }
            catch (MessageQueueException ex)
            {
                Console.WriteLine(ex);
            }
        }

        public void Start()
        {
            timer.Start();
        }

        public void Stop()
        {
            timer.Stop();
        }
    }
}
