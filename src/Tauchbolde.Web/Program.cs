using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace Tauchbolde.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .CaptureStartupErrors(true)
                .UseSetting("detailedErrors", "true")
                .UseApplicationInsights()
                .UseStartup<Startup>();
    }
}
