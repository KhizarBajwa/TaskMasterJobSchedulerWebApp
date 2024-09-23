using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.Repositories
{
    public interface IJobScheduleRepository
    {
        Task<IEnumerable<JobSchedule>> GetAllAsync();
        Task<JobSchedule> GetByIdAsync(int id);
        Task AddAsync(JobSchedule jobSchedule);
        Task UpdateAsync(JobSchedule jobSchedule);
        Task DeleteAsync(int id);
        Task<IEnumerable<JobSchedule>> GetActiveJobSchedulesAsync();
    }
}
