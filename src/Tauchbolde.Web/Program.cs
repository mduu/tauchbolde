using Microsoft.AspNetCore;

namespace Tauchbolde.Web
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        // ReSharper disable once MemberCanBePrivate.Global
        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.AddEnvironmentVariables();
                    config.AddJsonFile("appsettings.local.json", true);
                })
                .CaptureStartupErrors(true)
                .UseSetting("detailedErrors", "true")
                .UseStartup<Startup>();
    }
}
