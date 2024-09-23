using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities.TimesheetReminder;
using Core.Entities;
using Core.Interfaces.Repositories;
using Microsoft.Extensions.Logging;
using Quartz;
using Quartz.Impl;

namespace Presentation.Jobs.Service
{
    public class JobService : IJobService
    {
        private readonly IExecutionLogRepository _executionLogRepository;
        private readonly ISchedulerFactory _schedulerFactory;
        private readonly ILogger<JobService> _logger;

        public JobService(IExecutionLogRepository executionLogRepository, ISchedulerFactory schedulerFactory, ILogger<JobService> logger)
        {
            _executionLogRepository = executionLogRepository;
            _schedulerFactory = schedulerFactory;
            _logger = logger;
        }

        // Builds the JobDetail object based on the provided JobSchedule
        public JobDetailImpl BuildJobDetail(JobSchedule jobSchedule)
        {
            return (JobDetailImpl)JobBuilder.Create<TimesheetReminderJob>()
                .WithIdentity(jobSchedule.JobName, jobSchedule.JobGroup)
                .UsingJobData("JobName", jobSchedule.JobName)
                .UsingJobData("JobGroup", jobSchedule.JobGroup)
                .Build();
        }

        // Builds the Trigger object based on the provided JobSchedule
        public ITrigger BuildTrigger(JobSchedule jobSchedule)
        {
            ITrigger trigger = null;

            switch (jobSchedule.TriggerType)
            {
                case "Cron":
                    trigger = TriggerBuilder.Create()
                        .WithIdentity(jobSchedule.TriggerName, jobSchedule.JobGroup)
                        .WithCronSchedule(jobSchedule.CronTriggerExpression)
                        .Build();
                    break;

                case "Daily":
                    trigger = TriggerBuilder.Create()
                        .WithIdentity(jobSchedule.TriggerName, jobSchedule.JobGroup)
                        .WithDailyTimeIntervalSchedule(s => s.OnEveryDay())
                        .Build();
                    break;

                case "Simple":
                    trigger = TriggerBuilder.Create()
                        .WithIdentity(jobSchedule.TriggerName, jobSchedule.JobGroup)
                        .WithSimpleSchedule(s => s.WithIntervalInHours(24).RepeatForever())
                        .Build();
                    break;
            }

            return trigger;
        }

        // Pauses a job
        public async Task<bool> PauseJobAsync(string jobName, string jobGroup)
        {
            var scheduler = await _schedulerFactory.GetScheduler();
            var jobKey = new JobKey(jobName, jobGroup);

            if (await scheduler.CheckExists(jobKey))
            {
                await scheduler.PauseJob(jobKey);
                _logger.LogInformation("Job paused: {JobName} in group {JobGroup}", jobName, jobGroup);
                return true;
            }

            _logger.LogWarning("Job does not exist: {JobName} in group {JobGroup}", jobName, jobGroup);
            return false;
        }

        // Resumes a paused job
        public async Task<bool> ResumeJobAsync(string jobName, string jobGroup)
        {
            var scheduler = await _schedulerFactory.GetScheduler();
            var jobKey = new JobKey(jobName, jobGroup);

            if (await scheduler.CheckExists(jobKey))
            {
                await scheduler.ResumeJob(jobKey);
                _logger.LogInformation("Job resumed: {JobName} in group {JobGroup}", jobName, jobGroup);
                return true;
            }

            _logger.LogWarning("Job does not exist: {JobName} in group {JobGroup}", jobName, jobGroup);
            return false;
        }

        // Manually triggers (runs) a job
        public async Task<bool> RunJobAsync(string jobName, string jobGroup)
        {
            var scheduler = await _schedulerFactory.GetScheduler();
            var jobKey = new JobKey(jobName, jobGroup);

            if (!await scheduler.CheckExists(jobKey))
            {
                _logger.LogWarning("Job does not exist: {JobName} in group {JobGroup}", jobName, jobGroup);
                return false;
            }

            await scheduler.TriggerJob(jobKey);
            _logger.LogInformation("Job manually triggered: {JobName} in group {JobGroup}", jobName, jobGroup);

            // Log the execution
            var log = new ExecutionLog
            {
                JobName = jobName,
                JobGroup = jobGroup,
                FireTimeUtc = DateTimeOffset.UtcNow,
                Result = "Triggered Manually",
                IsSuccess = true,
                DateAddedUtc = DateTimeOffset.UtcNow
            };

            await _executionLogRepository.AddAsync(log);

            return true;
        }

        // Deletes a job
        public async Task<bool> DeleteJobAsync(string jobName, string jobGroup)
        {
            var scheduler = await _schedulerFactory.GetScheduler();
            var jobKey = new JobKey(jobName, jobGroup);

            if (await scheduler.CheckExists(jobKey))
            {
                await scheduler.DeleteJob(jobKey);
                _logger.LogInformation("Job deleted: {JobName} in group {JobGroup}", jobName, jobGroup);
                return true;
            }

            _logger.LogWarning("Job does not exist: {JobName} in group {JobGroup}", jobName, jobGroup);
            return false;
        }

        // Stops a job (marks it as canceled)
        public async Task<bool> StopJobAsync(string jobName, string jobGroup)
        {
            var scheduler = await _schedulerFactory.GetScheduler();
            var jobKey = new JobKey(jobName, jobGroup);

            if (await scheduler.CheckExists(jobKey))
            {
                await scheduler.PauseJob(jobKey);

                var log = new ExecutionLog
                {
                    JobName = jobName,
                    JobGroup = jobGroup,
                    FireTimeUtc = DateTimeOffset.UtcNow,
                    Result = "Stopped",
                    IsSuccess = false,
                    DateAddedUtc = DateTimeOffset.UtcNow
                };

                await _executionLogRepository.AddAsync(log);

                _logger.LogInformation("Job stopped: {JobName} in group {JobGroup}", jobName, jobGroup);
                return true;
            }

            _logger.LogWarning("Job does not exist: {JobName} in group {JobGroup}", jobName, jobGroup);
            return false;
        }

        // Clones a job
        public JobSchedule CloneJob(JobSchedule originalJob)
        {
            var clonedJob = new JobSchedule
            {
                JobName = originalJob.JobName + "_Clone",
                JobGroup = originalJob.JobGroup,
                CronTriggerExpression = originalJob.CronTriggerExpression,
                TriggerType = originalJob.TriggerType,
                TriggerName = originalJob.TriggerName + "_Clone"
            };

            return clonedJob;
        }
    }

}
