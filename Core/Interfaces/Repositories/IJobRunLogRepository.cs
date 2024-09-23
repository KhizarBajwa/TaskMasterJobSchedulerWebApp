using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.Repositories
{
    public interface IJobRunLogRepository
    {
        Task<IEnumerable<JobRunLog>> GetAllAsync();
        Task<JobRunLog> GetByIdAsync(int id);
        Task AddAsync(JobRunLog jobRunLog);
        Task UpdateAsync(JobRunLog jobRunLog);
        Task DeleteAsync(int id);
    }
}
