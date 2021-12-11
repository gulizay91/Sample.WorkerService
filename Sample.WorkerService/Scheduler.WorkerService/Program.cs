using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Scheduler.WorkerService.Hangfire;


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
                            var configuration = services.BuildServiceProvider().GetRequiredService<IConfiguration>();
                            services.AddHangfireConfigurationServices(configuration);
                        })
                        .ConfigureWebHostDefaults(webBuilder =>
                        {
                            webBuilder.Configure(app =>
                            {
                                app.AddHangfireConfiguration();
                                app.UseHealthChecks("/hc");
                            });
                            webBuilder.UseUrls("http://localhost:9600", "https://localhost:9601");
                        }).Build();

await host.RunAsync();