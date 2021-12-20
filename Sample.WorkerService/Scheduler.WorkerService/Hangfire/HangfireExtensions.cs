using Ardalis.GuardClauses;
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

            HangfireConfig hangfireConfig = configuration.GetSection("HangfireConfig").Get<HangfireConfig>();
            Guard.Against.Null<HangfireConfig>(hangfireConfig, nameof(hangfireConfig));

            FireAndForgetJobs.GenerateFakeEmployees();

            var sendThanksMailConfig = hangfireConfig.Jobs.Where(r => r.Name == "SendThanksMail").FirstOrDefault();
            Guard.Against.Null<Jobs>(sendThanksMailConfig, nameof(sendThanksMailConfig));
            if(sendThanksMailConfig.Enable)
                ReccuringJobs.SendThanksMail(sendThanksMailConfig.CronExpression);

            var remindDrinkWaterConfig = hangfireConfig.Jobs.Where(r => r.Name == "RemindDrinkWater").FirstOrDefault();
            Guard.Against.Null<Jobs>(remindDrinkWaterConfig, nameof(remindDrinkWaterConfig));
            ReccuringJobs.RemindDrinkWater(remindDrinkWaterConfig.CronExpression);

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
