using Core.Entities.TimesheetReminder;
using Core.Interfaces.Repositories;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class ExecutionLogRepository : IExecutionLogRepository
    {
        private readonly ApplicationDbContext _context;

        public ExecutionLogRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // Get all execution logs
        public async Task<IEnumerable<ExecutionLog>> GetAllAsync()
        {
            return await _context.ExecutionLogs.OrderByDescending(log => log.DateAddedUtc).ToListAsync();
        }

        // Get an execution log by its ID
        public async Task<ExecutionLog> GetByIdAsync(long logId)
        {
            return await _context.ExecutionLogs.FindAsync(logId);
        }

        // Get logs by job name
        public async Task<IEnumerable<ExecutionLog>> GetLogsByJobNameAsync(string jobName)
        {
            return await _context.ExecutionLogs
                                 .Where(log => log.JobName == jobName)
                                 .OrderByDescending(log => log.DateAddedUtc)
                                 .ToListAsync();
        }

        // Add a new execution log
        public async Task AddAsync(ExecutionLog log)
        {
            await _context.ExecutionLogs.AddAsync(log);
            await _context.SaveChangesAsync();
        }

        // Update an existing execution log
        public async Task UpdateAsync(ExecutionLog log)
        {
            _context.ExecutionLogs.Update(log);
            await _context.SaveChangesAsync();
        }

        // Delete an execution log by its ID
        public async Task DeleteAsync(long logId)
        {
            var log = await GetByIdAsync(logId);
            if (log != null)
            {
                _context.ExecutionLogs.Remove(log);
                await _context.SaveChangesAsync();
            }
        }
    }
}
