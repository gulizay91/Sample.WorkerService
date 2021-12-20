# Sample.WorkerService

## Tech
[.NET 6](https://dotnet.microsoft.com/download/dotnet/6.0)

KeyWords: 
  * WorkerService
  * Hangfire
  * SQLite
  * Bogus for fake data


## About The Project
Sample hangfire service by worker service

![Hangfire dashboard and console log](/hangfire00.png?raw=true "hangfire dashboard")
##
![System tray icon for Reminder Notify](/hangfire01.png?raw=true "System tray icon")

## Create Windows Service
```bash
sc.exe create SchedulerWorkerService binPath="<publish folder>\Scheduler.WorkerService.exe"
```

## License

MIT