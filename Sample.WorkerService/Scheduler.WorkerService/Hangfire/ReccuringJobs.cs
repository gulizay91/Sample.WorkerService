﻿using Hangfire;
using Scheduler.WorkerService.Faker;

namespace Scheduler.WorkerService.Hangfire
{
    internal static class ReccuringJobs
    {
        //[AutomaticRetry(Attempts = 2)]
        internal static Task SendThanksMail()
        {
            //var cronExpression = "0 1 * * *";
            var cronExpression = "*/2 * * * *";
            RecurringJob.AddOrUpdate(nameof(SendThanksMail), () => ReccuringJobProcess.SendThanksMail(), cronExpression);
            return Task.CompletedTask;
        }

        //[AutomaticRetry(Attempts = 2)]
        //[DisableConcurrentExecution(2)]
        internal static Task RemindDrinkWater()
        {
            //var cronExpression = "0 1 * * *";
            var cronExpression = "*/1 * * * *";
            RecurringJob.AddOrUpdate(nameof(RemindDrinkWater), () => ReccuringJobProcess.RemindDrinkWater(), cronExpression);
            return Task.CompletedTask;
        }
    }

    internal static class ReccuringJobProcess
    {
        public static void SendThanksMail()
        {
            Console.WriteLine($"start count: {GeneratorFakeData.Employees.Count}, time: {DateTime.Now}");
            List<Employee> removeEmployees = new();
            GeneratorFakeData.Employees.ForEach(employee => {
                var anniversary = DateTime.Now.Year - employee.JoinedDate.Year;
                if (anniversary >= 10)
                {
                    Console.WriteLine($"Happy anniversary {employee.Name} {employee.Surname}. It's been {anniversary} years. Thanks for your hard work.");
                    removeEmployees.Add(employee);
                }
            });
            GeneratorFakeData.Employees.RemoveAll(item => removeEmployees.Contains(item));
            Console.WriteLine($"end count: {GeneratorFakeData.Employees.Count}, time: {DateTime.Now}");
        }
    
        public static void RemindDrinkWater()
        {
            string title = "DrinkWater NotifyIcon";
            string text = "get up and drink water!";

            Console.WriteLine(text);

            //TaskbarIcon taskbarIcon = new();
            //taskbarIcon.ShowBalloonTip(title, text, BalloonIcon.Warning);
        }
    }
}
