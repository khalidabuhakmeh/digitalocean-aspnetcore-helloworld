using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using StackExchange.Redis;

namespace WebApplication4
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            // set up redis
            services.AddSingleton(s => 
                ConnectionMultiplexer.Connect(new ConfigurationOptions
                {
                    User = Configuration["REDISUSER"],
                    Password = Configuration["REDISPASSWORD"],
                    EndPoints = { Configuration["REDISHOST"], Configuration["REDISPORT"] }
                }));
            
            // return IDatabase
            services.AddScoped(s => {
                var connection = s.GetRequiredService<ConnectionMultiplexer>();
                return connection.GetDatabase();
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    var db = context.RequestServices.GetRequiredService<IDatabase>();
                    var result =
                        await db.StringIncrementAsync("count");
                    
                    await context.Response.WriteAsync($"Hello World! Counting at {result}");
                });
            });
        }
    }
}