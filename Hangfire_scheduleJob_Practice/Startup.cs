using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hangfire;
using Hangfire.MemoryStorage;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Hangfire_scheduleJob_Practice
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHangfire(config =>
            {
                config.SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer().UseDefaultTypeSerializer()
               .UseMemoryStorage();
            });

            services.AddHangfireServer();


            services.AddSingleton<IJobPrint, JobPrint>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env
            ,IBackgroundJobClient backgroundJobClient,
            IRecurringJobManager recurringJobManager
            ,IServiceProvider serviceProvider  //  Get bind Scheduing Job with dependency Injection... 
             )
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
                    await context.Response.WriteAsync("Hello World!");
                });
            });



            app.UseHangfireDashboard();
            /// for Scheduling the Job we will add the backgourndjobcliend

            backgroundJobClient.Enqueue(() => Console.WriteLine("Hello, this is our First Job...!"));

            /// For creating recuring  Job..  Add  IRecurringJobManager
            /// 

            //recurringJobManager.AddOrUpdate("Run Every minutes", 
            //    () => Console.WriteLine("This Job will execute after every minute"), "* * * * *");

            // recurringJobManager.AddOrUpdate("Run Every minutes",
            //() => Console.WriteLine("This SEcond Job will execute after every minute"), "* * * * *");


            // we can us it as a part of dependency injection as well. lets create jobPrint class as see what happen


            recurringJobManager.AddOrUpdate("Run Every minutes",
                () => serviceProvider.GetService<IJobPrint>().HangfirePrintJob(), "* * * * *");

        }
    }
}
