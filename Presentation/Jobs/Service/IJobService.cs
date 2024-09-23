using Core.Entities;
using Quartz;
using Quartz.Impl;

namespace Presentation.Jobs.Service
{
    public interface IJobService
    {
        JobDetailImpl BuildJobDetail(JobSchedule jobSchedule);        
        Task<bool> PauseJobAsync(string jobName, string jobGroup);
        Task<bool> ResumeJobAsync(string jobName, string jobGroup);
        Task<bool> RunJobAsync(string jobName, string jobGroup);
        Task<bool> DeleteJobAsync(string jobName, string jobGroup);
        Task<bool> StopJobAsync(string jobName, string jobGroup);
        JobSchedule CloneJob(JobSchedule originalJob);
    }
}
