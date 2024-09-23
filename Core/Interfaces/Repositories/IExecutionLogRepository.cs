using Core.Entities.TimesheetReminder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.Repositories
{
    public interface IExecutionLogRepository
    {
        Task<IEnumerable<ExecutionLog>> GetAllAsync();
        Task<ExecutionLog> GetByIdAsync(long logId);
        Task<IEnumerable<ExecutionLog>> GetLogsByJobNameAsync(string jobName);
        Task AddAsync(ExecutionLog log);
        Task UpdateAsync(ExecutionLog log);
        Task DeleteAsync(long logId);
    }
}
