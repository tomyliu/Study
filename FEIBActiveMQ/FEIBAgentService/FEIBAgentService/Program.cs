using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]
namespace FEIBAgentService
{
    static class Program
    {
        /// <summary>
        /// 應用程式的主要進入點。
        /// </summary>
        static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new AgentService()
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
