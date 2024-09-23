using Core.Entities;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Infrastructure.Repositories;
using Quartz;

namespace Presentation.Jobs
{
    public class TimesheetReminderJob : IJob
    {
        private readonly IJobRunLogRepository _jobRunLogRepository;
        private readonly IJobScheduleRepository _jobScheduleRepository;
        private readonly INotificationService _notificationService;
        public TimesheetReminderJob(IJobRunLogRepository jobRunLogRepository, IJobScheduleRepository jobScheduleRepository, INotificationService notificationService)
        {
            _jobRunLogRepository = jobRunLogRepository;
            _jobScheduleRepository = jobScheduleRepository;
            _notificationService = notificationService;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            // Log the start time of the job execution
            var startTime = DateTime.Now;

            try
            {
                // Fetch job schedule details using context.JobDetail.Key
                var jobKey = context.JobDetail.Key;
                var jobSchedule = await _jobScheduleRepository.GetByIdAsync(Convert.ToInt32(jobKey.Name));

                if (jobSchedule == null || jobSchedule.IsCancelled)
                {
                    Console.WriteLine("Job is either cancelled or not found.");
                    return;
                }

                // Log job execution start in JobRunLog
                var jobRunLog = new JobRunLog
                {
                    JobSchedule_JobId = jobSchedule.JobId,
                    StartDate = startTime,
                    StatusId = 1, // 1 for 'Running'
                    Server = Environment.MachineName
                };

                await _jobRunLogRepository.AddAsync(jobRunLog);

                // Execute the job logic - sending timesheet reminders
                _notificationService.SendTimeSheetReminderEmail(jobRunLog.JobRunId);

                // Update job log as success after sending the reminder
                jobRunLog.StatusId = 2; // 2 for 'Completed'
                jobRunLog.EndDate = DateTime.Now;
                await _jobRunLogRepository.UpdateAsync(jobRunLog);
            }
            catch (Exception ex)
            {
                // If any exception occurs, log it as a failure
                var jobRunLog = new JobRunLog
                {
                    JobSchedule_JobId = Convert.ToInt32(context.JobDetail.Key.Name),
                    StartDate = startTime,
                    EndDate = DateTime.Now,
                    StatusId = 3, // 3 for 'Failed'
                    Server = Environment.MachineName
                };

                await _jobRunLogRepository.AddAsync(jobRunLog);
                Console.WriteLine($"Error executing TimesheetReminderJob: {ex.Message}");
            }
        }
    }
}
