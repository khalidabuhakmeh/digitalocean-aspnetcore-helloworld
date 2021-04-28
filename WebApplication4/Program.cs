using System.Net;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using static System.Environment;

namespace WebApplication4
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    // for railway apps
                    if (int.TryParse(GetEnvironmentVariable("PORT"), out var port))
                    {
                        webBuilder.ConfigureKestrel(k =>  {
                            k.Listen(IPAddress.Loopback, port);
                            k.Listen(IPAddress.Loopback, 80);
                        });
                    }
                    
                    webBuilder.UseStartup<Startup>();
                });
    }
}