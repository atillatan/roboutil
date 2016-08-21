using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Threading.Tasks;
 
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace RoboUtil.Common.Service
{
    public class Startup
    {
        public IConfigurationRoot Configuration { get; }
        public Startup(IHostingEnvironment env)
        {
            var configurationBuilder = new ConfigurationBuilder()
            .SetBasePath(env.ContentRootPath)
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
            .AddEnvironmentVariables();

            if (env.IsDevelopment())//yazilim gelistirme ortami
            {
                //Atilla:gelistirme ortaminda configurasyonumuz farkli, diger ortamlarda farkli olacak
            }
            else if (env.IsProduction())//canli ortam
            {
            }
            else if (env.IsStaging())//Test ortami
            {

            }

            Configuration = configurationBuilder.Build();
        }
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddMvc();
            //services.AddTransient<IConfiguration>(Configuration); net46
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));//Atilla:Log4Net ile replace edilecek

            loggerFactory.AddDebug();

            //app.UseIISPlatformHandler();net46 
            app.UseStaticFiles();

            app.UseMvc();

          //app.UseRequestHandler();//Atilla:yapim asamasinda

            //ServiceStartup.Start(new FileInfo(env.WebRootPath + "\\App_Data\\Usis.config"));
        }

       // public static void Main(string[] args) => WebApplication.Run<Startup>(args); net46 
    }
}
