using Serilog;
using Serilog.Events;

namespace WebsiteStatus;

public class Program
{
    public static void Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
            .Enrich.FromLogContext()
            .WriteTo.File(@"C:\temp\workerservice\LogFile.txt")
            .CreateLogger();

        IHost host = Host.CreateDefaultBuilder(args)
            .UseWindowsService()
            .UseSerilog()
            .ConfigureServices(services =>
            {
                services.AddHostedService<Worker>();
            })
            .Build();

        try
        {
            Log.Information("Starting up the service");
            host.Run();

        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "There was a problem starting the serivce");
            return;
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }
}