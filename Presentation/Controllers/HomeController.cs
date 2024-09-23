using Core.Interfaces.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Presentation.Jobs.Service;
using Presentation.Models;
using Quartz;
using System.Diagnostics;

namespace Presentation.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IExecutionLogRepository _executionLogRepository;
        private readonly IJobScheduleRepository _jobScheduleRepository;
        private readonly ISchedulerFactory _schedulerFactory;
        private readonly IJobService _jobService;

        public HomeController(ILogger<HomeController> logger, IExecutionLogRepository executionLogRepository,
            ISchedulerFactory schedulerFactory, IJobScheduleRepository jobScheduleRepository, IJobService jobService)
        {
            _logger = logger;
            _executionLogRepository = executionLogRepository;
            _schedulerFactory = schedulerFactory;
            _jobScheduleRepository = jobScheduleRepository;
            _jobService = jobService;
        }

        public async Task<IActionResult> Index()
        {
            var logs = await _executionLogRepository.GetAllAsync();
            var jobs = await _jobScheduleRepository.GetAllAsync();
            ViewBag.JobSchedules = jobs;

            var scheduler = await _schedulerFactory.GetScheduler();
            var currentlyExecutingJobs = await scheduler.GetCurrentlyExecutingJobs();
            ViewBag.CurrentlyExecutingJobs = currentlyExecutingJobs;

            return View(logs);
        }





        // Pause a job
        public async Task<IActionResult> PauseJob(string jobName, string jobGroup)
        {
            var result = await _jobService.PauseJobAsync(jobName, jobGroup);
            if (result)
            {
                _logger.LogInformation("Job paused: {JobName}", jobName);
            }
            else
            {
                _logger.LogWarning("Failed to pause job: {JobName}", jobName);
            }

            return RedirectToAction(nameof(Index));
        }

        // Resume a job
        public async Task<IActionResult> ResumeJob(string jobName, string jobGroup)
        {
            var result = await _jobService.ResumeJobAsync(jobName, jobGroup);
            if (result)
            {
                _logger.LogInformation("Job resumed: {JobName}", jobName);
            }
            else
            {
                _logger.LogWarning("Failed to resume job: {JobName}", jobName);
            }

            return RedirectToAction(nameof(Index));
        }

        // Manually run a job
        public async Task<IActionResult> RunJob(string jobName, string jobGroup)
        {
            var result = await _jobService.RunJobAsync(jobName, jobGroup);
            if (result)
            {
                _logger.LogInformation("Job manually triggered: {JobName}", jobName);
            }
            else
            {
                _logger.LogWarning("Failed to manually trigger job: {JobName}", jobName);
            }

            return RedirectToAction(nameof(Index));
        }



        // Stop (cancel) a job
        public async Task<IActionResult> StopJob(string jobName, string jobGroup)
        {
            var result = await _jobService.StopJobAsync(jobName, jobGroup);
            if (result)
            {
                _logger.LogInformation("Job stopped: {JobName}", jobName);
            }
            else
            {
                _logger.LogWarning("Failed to stop job: {JobName}", jobName);
            }

            return RedirectToAction(nameof(Index));
        }



        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
