using Hangfire;
using Scheduler.WorkerService.Faker;

namespace Scheduler.WorkerService.Hangfire
{
    public static class FireAndForgetJobs
    {
        internal static Task GenerateFakeEmployees()
        {
            BackgroundJob.Enqueue(() => FireAndForgetJobProcess.GetEmployeeList());
            return Task.CompletedTask;
        }
    }

    public static class FireAndForgetJobProcess
    {
        public static void GetEmployeeList()
        {
            GeneratorFakeData.GenerateSimpleEmployeeList(200);
        }
    }

}
