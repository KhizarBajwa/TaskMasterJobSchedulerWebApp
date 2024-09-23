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
    public class JobScheduleRepository : IJobScheduleRepository
    {
        private readonly ApplicationDbContext _context;

        public JobScheduleRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<JobSchedule>> GetActiveJobSchedulesAsync()
        {
            return await _context.JobSchedules
                                 .Where(js => js.IsActive)
                                 .ToListAsync();
        }

        public async Task<IEnumerable<JobSchedule>> GetAllAsync()
        {
            return await _context.JobSchedules.ToListAsync();
        }

        public async Task<JobSchedule> GetByIdAsync(int id)
        {
            return await _context.JobSchedules.FindAsync(id);
        }

        public async Task AddAsync(JobSchedule jobSchedule)
        {
            await _context.JobSchedules.AddAsync(jobSchedule);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(JobSchedule jobSchedule)
        {
            _context.JobSchedules.Update(jobSchedule);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var jobSchedule = await _context.JobSchedules.FindAsync(id);
            if (jobSchedule != null)
            {
                _context.JobSchedules.Remove(jobSchedule);
                await _context.SaveChangesAsync();
            }
        }
    }
}
