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
                            k.Listen(IPAddress.None, port);
                            k.Listen(IPAddress.None, 80);
                        });
                    }
                    
                    webBuilder.UseStartup<Startup>();
                });
    }
}