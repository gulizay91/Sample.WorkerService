using Hangfire;
using Hangfire.Storage.SQLite;
using Microsoft.AspNetCore.Builder;

namespace Scheduler.WorkerService.Hangfire
{
    public static class HangfireExtensions
    {
        public static IServiceCollection AddHangfireConfigurationServices(this IServiceCollection services, IConfiguration configuration)
        {
            //string connectionString = configuration.GetConnectionString("SQLDatabaseConnection");
            //var options = new SqlServerStorageOptions() { PrepareSchemaIfNecessary = true };
            //GlobalConfiguration.Configuration.UseSqlServerStorage(connectionString, options);
            //services.AddHangfire(configuration => configuration.UseSqlServerStorage(connectionString, options));
            string connectionString = configuration.GetConnectionString("SQLiteDatabaseConnection");

            // Hangfire.SQLite issue recurring job running multiple times sqlite
            // ref: https://discuss.hangfire.io/t/hangfire-with-sqlite-storage-executing-multiple-instances-of-recurring-jobs/7620/3
            var options = new SQLiteStorageOptions();//new SQLiteStorageOptions() { PrepareSchemaIfNecessary = true }; => Hangfire.SQLite
            GlobalConfiguration.Configuration.UseSQLiteStorage(connectionString, options);
            services.AddHangfire(configuration => configuration.UseSQLiteStorage(connectionString, options));

            services.AddHangfireServer();

            FireAndForgetJobs.GenerateFakeEmployees();
            ReccuringJobs.SendThanksMail();
            ReccuringJobs.RemindDrinkWater();

            return services;
        }

        internal static IApplicationBuilder AddHangfireConfiguration(this IApplicationBuilder applicationBuilder)
        {
            applicationBuilder.UseHangfireDashboard();
            applicationBuilder.ApplicationServices.GetRequiredService<IBackgroundJobClient>();
            applicationBuilder.ApplicationServices.GetService<IRecurringJobManager>();
            return applicationBuilder;
        }
    }
}
