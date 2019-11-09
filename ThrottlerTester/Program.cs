using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Messaging;
using System.Text;
using System.Threading.Tasks;
using CommandLine;
namespace ThrottlerTester
{
    class Program
    {
        public class Options
        {
            [Option('c', "count", Required = false, HelpText = "Count of elements to queue", Default = 10)]
            public int Count { get; set; }
        }
        static void Main(string[] args)
        {
            MessageQueue queue = new MessageQueue(".\\private$\\throttlerTest");
            Parser.Default.ParseArguments<Options>(args)
                .WithParsed(option =>
                {
                    var list = Get(option.Count);
                    var data = JsonConvert.SerializeObject(list);
                    queue.Send(data, MessageQueueTransactionType.None);
                });
        }

        static List<int> Get(int n)
        {
            List<int> list = new List<int>(n);
            Random r = new Random();
            for (int i = 0; i < n; i++)
            {
                list.Add(i);
            }
            return list;
        }

    }
}
