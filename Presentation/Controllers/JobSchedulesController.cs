using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Core.Entities;
using Infrastructure.Data;
using Core.Interfaces.Repositories;

namespace Presentation.Controllers
{
    public class JobSchedulesController : Controller
    {
        private readonly IJobScheduleRepository _jobScheduleRepository;

        public JobSchedulesController(ApplicationDbContext context, IJobScheduleRepository jobScheduleRepository)
        {
            _jobScheduleRepository = jobScheduleRepository;
        }

        // GET: JobSchedules
        public async Task<IActionResult> Index()
        {
            return View(await _jobScheduleRepository.GetAllAsync());
        }

        // GET: JobSchedules/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jobSchedule = await _jobScheduleRepository.GetByIdAsync(id.Value);                
            if (jobSchedule == null)
            {
                return NotFound();
            }

            return View(jobSchedule);
        }

        // GET: JobSchedules/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: JobSchedules/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("JobName,JobGroup,Description,NumberOfThreads,TriggerName,TriggerType,CronTriggerExpression,IsCancelled")] JobSchedule jobSchedule)
        {
            jobSchedule.IsActive = true;
            jobSchedule.CreatedAt = DateTime.Now;

            if (ModelState.IsValid)
            {
                await _jobScheduleRepository.AddAsync(jobSchedule);                
                return RedirectToAction(nameof(Index));
            }
            return View(jobSchedule);
        }

        // GET: JobSchedules/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jobSchedule = await _jobScheduleRepository.GetByIdAsync(id.Value);
            if (jobSchedule == null)
            {
                return NotFound();
            }
            return View(jobSchedule);
        }

        // POST: JobSchedules/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("JobId,JobName,JobGroup,Description,NumberOfThreads,CronTriggerExpression,IsCancelled")] JobSchedule jobSchedule)
        {
            if (id != jobSchedule.JobId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _jobScheduleRepository.UpdateAsync(jobSchedule);                    
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await JobScheduleExists(jobSchedule.JobId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(jobSchedule);
        }

        // GET: JobSchedules/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jobSchedule = await _jobScheduleRepository.GetByIdAsync(id.Value);
            if (jobSchedule == null)
            {
                return NotFound();
            }

            return View(jobSchedule);
        }

        // POST: JobSchedules/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var jobSchedule = await _jobScheduleRepository.GetByIdAsync(id);
            if (jobSchedule != null)
            {
                await _jobScheduleRepository.DeleteAsync(id);
            }
            
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> JobScheduleExists(int id)
        {
            return (await _jobScheduleRepository.GetByIdAsync(id)) != null ? true : false;
        }
    }
}
