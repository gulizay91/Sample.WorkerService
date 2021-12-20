namespace Scheduler.WorkerService.Hangfire
{
    public class HangfireConfig
    {
        public List<Jobs> Jobs { get; set; }
    }

    public class Jobs
    {
        public string Name { get; set; }
        public bool Enable { get; set; }
        public string CronExpression { get; set; }
    }
}
