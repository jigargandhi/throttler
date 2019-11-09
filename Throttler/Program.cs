using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topshelf;

namespace Throttler
{
    class Program
    {
        static void Main(string[] args)
        {
            var apiUrl = ConfigurationManager.AppSettings["apiUrl"].ToString();
            var queuePath = ConfigurationManager.AppSettings["queueName"].ToString();
            ApiClient client = new ApiClient(apiUrl);

            var rc = HostFactory.Run(x =>                                  //1
            {
                x.Service<Orchestrator>(s =>                                   //2
                {
                    s.ConstructUsing(name => new Orchestrator(client, queuePath, 2));                //3
                    s.WhenStarted(tc => tc.Start());                         //4
                    s.WhenStopped(tc => tc.Stop());                          //5
                });
                x.RunAsLocalSystem();                                       //6

                x.SetDescription("Orchestrator clone Messaging Service");                   //7
                x.SetDisplayName("Orchestrator clone Messaging Service");                                  //8
                x.SetServiceName("Orchestrator clone Messaging Service");                                  //9
            });                                                             //10

            var exitCode = (int)Convert.ChangeType(rc, rc.GetTypeCode());  //11
            Environment.ExitCode = exitCode;
        }
    }
}
