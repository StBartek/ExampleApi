using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Hosting.WindowsServices;
using Microsoft.Extensions.Logging;
using NLog.Web;
using System;
using System.IO;
using System.Runtime.InteropServices;

namespace TestApi
{
    public class Program
    {
        private static readonly NLog.Logger logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
        public static void Main(string[] args)
        {
            try
            {
                logger.Debug($"Start microservice: 'TestApi' on OS: '{RuntimeInformation.OSDescription}'");
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception exception)
            {
                //NLog: catch setup errors
                logger.Error(exception, "Stopped microservice because of exception");
                throw;
            }
            finally
            {
                // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
                logger.Debug("Stop microservice: TestApi");
                NLog.LogManager.Shutdown();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            var _config = new ConfigurationBuilder().SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                           .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true).Build();

            var isWindowsService = WindowsServiceHelpers.IsWindowsService();

            var hostUrls = _config.GetValue<string>("RestApiHost:urls");
            var envPortenvPort = System.Environment.GetEnvironmentVariable("PORT");
            logger.Info($"Rest service urls: {hostUrls}, env port: {envPortenvPort}");

            var builder = Host.CreateDefaultBuilder(args)
                .ConfigureHostConfiguration(configHost =>
                {
                    configHost.SetBasePath(Directory.GetCurrentDirectory());
                })
               .ConfigureAppConfiguration((hostingContext, config) =>
               {
                   var env = hostingContext.HostingEnvironment;
                   config.AddConfiguration(_config);
                   config.AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);
                   config.AddEnvironmentVariables();
               })
               .ConfigureWebHostDefaults(webBuilder =>
               {
                   webBuilder.UseStartup<Startup>();

                   var urls = string.IsNullOrEmpty(envPortenvPort) ? hostUrls.Split(';') : new string[] { "http://*:" + envPortenvPort };

                   //webBuilder.UseUrls(hostUrls.Split(';'));
                   webBuilder.UseUrls(urls);
               })
               .ConfigureLogging(logging =>
               {
                   logging.ClearProviders();
                   logging.SetMinimumLevel(LogLevel.Trace);
               })
               .UseContentRoot(AppDomain.CurrentDomain.BaseDirectory)
               .UseNLog();

            if (isWindowsService)
            {
                logger.Info("Running as Windows Service!");
                builder.UseWindowsService();
            }

            return builder;
        }
    }
}
