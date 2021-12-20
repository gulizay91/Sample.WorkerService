using Ardalis.GuardClauses;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Scheduler.WorkerService.Hangfire;

var configuration = new ConfigurationBuilder()
        .AddEnvironmentVariables()
        .AddCommandLine(args)
        .AddJsonFile("appsettings.json")
        .Build();

IHost host = Host.CreateDefaultBuilder(args)
                        .UseWindowsService()
                        .ConfigureServices(services =>
                        {
                            //services.AddHostedService<Worker>();
                            services.AddHealthChecks();
                            //services.AddLogging(builder => builder
                            //        .AddDebug()
                            //        .AddConsole()
                            //);
                            services.AddHangfireConfigurationServices(configuration);
                        })
                        .ConfigureWebHostDefaults(webBuilder =>
                        {
                            webBuilder.Configure(app =>
                            {
                                app.AddHangfireConfiguration();
                                app.UseHealthChecks("/hc");
                            });

                            //string[] urls = new string[] { "http://localhost:9600", "https://localhost:9601" };
                            string[] urls = configuration.GetSection("HangfireConfig:Urls").Get<string[]>();
                            Guard.Against.Null<string[]>(urls, nameof(urls));
                            webBuilder.UseUrls(urls);
                        }).Build();

await host.RunAsync();