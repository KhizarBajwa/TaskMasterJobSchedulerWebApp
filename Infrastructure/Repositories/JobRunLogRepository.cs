using Core.Entities;
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
    public class JobRunLogRepository : IJobRunLogRepository
    {
        private readonly ApplicationDbContext _context;

        public JobRunLogRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<JobRunLog>> GetAllAsync()
        {
            return await _context.JobRunLogs
                .Include(jr => jr.JobSchedule) // Include the JobSchedule navigation property
                .ToListAsync();
        }

        public async Task<JobRunLog> GetByIdAsync(int id)
        {
            return await _context.JobRunLogs
                .Include(jr => jr.JobSchedule) // Include JobSchedule when retrieving by id
                .FirstOrDefaultAsync(jr => jr.JobRunId == id);
        }

        public async Task AddAsync(JobRunLog jobRunLog)
        {
            await _context.JobRunLogs.AddAsync(jobRunLog);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(JobRunLog jobRunLog)
        {
            _context.JobRunLogs.Update(jobRunLog);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var jobRunLog = await _context.JobRunLogs.FindAsync(id);
            if (jobRunLog != null)
            {
                _context.JobRunLogs.Remove(jobRunLog);
                await _context.SaveChangesAsync();
            }
        }
    }
}
