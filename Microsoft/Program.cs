
using Microsoft;
using System.ServiceProcess;

var service = new CustomService();

if(Environment.UserInteractive)
{
    service.Start();
    Console.WriteLine("Press any key to stop the service...");
    Console.ReadKey();
    service.Stop();
}

ServiceBase.Run(service);