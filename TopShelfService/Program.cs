using Topshelf;
using TopShelfService;

HostFactory.Run(x =>
{
    x.Service<CustomService>();
    x.SetServiceName("TopShelfService");
    x.SetDisplayName("Top Shelf Service");


    x.EnableServiceRecovery(r =>
    {
        r.RestartService(1) // Restart the service after 1 minute
        .RestartService(5) // Restart the service after 5 minutes
        .TakeNoAction();

        r.SetResetPeriod(1); // Reset the failure count after 1 day
    });

    x.DependsOn("MicrosoftCustomService");


    x.RunAsLocalSystem();
    x.StartAutomaticallyDelayed();
});