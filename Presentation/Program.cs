using Core.Interfaces.Repositories;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Configuration;
using Quartz;
using Presentation.Jobs;
using Presentation.Jobs.Service;


var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


//Add repository to the container.
builder.Services.AddScoped<IJobScheduleRepository, JobScheduleRepository>();
builder.Services.AddScoped<IJobRunLogRepository, JobRunLogRepository>();
builder.Services.AddScoped<IExecutionLogRepository, ExecutionLogRepository>();
builder.Services.AddScoped<IJobService, JobService>();

// Add Quartz services
builder.Services.AddQuartz(q =>
{
    // Use DI for jobs
    q.UseMicrosoftDependencyInjectionJobFactory();
});



// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// Dynamically schedule jobs from the database
using (var scope = app.Services.CreateScope())
{
    var scheduler = await scope.ServiceProvider.GetRequiredService<ISchedulerFactory>().GetScheduler();
    var jobScheduleRepository = scope.ServiceProvider.GetRequiredService<IJobScheduleRepository>();

    var jobSchedules = await jobScheduleRepository.GetActiveJobSchedulesAsync();

    foreach (var jobSchedule in jobSchedules)
    {
        var jobKey = new JobKey(jobSchedule.JobName, jobSchedule.JobGroup);

        if (await scheduler.CheckExists(jobKey))
        {
            continue; // Job already exists, skip scheduling
        }

        var jobDetail = JobBuilder.Create<TimesheetReminderJob>() // or whatever job class you have
                                  .WithIdentity(jobKey)
                                  .Build();

        ITrigger trigger = null;

        // Create the appropriate trigger based on the trigger type
        if (jobSchedule.TriggerType == "Cron")
        {
            trigger = TriggerBuilder.Create()
                                    .WithIdentity(jobSchedule.TriggerName, jobSchedule.JobGroup)
                                    .WithCronSchedule(jobSchedule.CronTriggerExpression)
                                    .Build();
        }
        else if (jobSchedule.TriggerType == "Daily")
        {
            trigger = TriggerBuilder.Create()
                                    .WithIdentity(jobSchedule.TriggerName, jobSchedule.JobGroup)
                                    .WithDailyTimeIntervalSchedule(x => x.OnEveryDay())
                                    .Build();
        }
        else if (jobSchedule.TriggerType == "Simple")
        {
            trigger = TriggerBuilder.Create()
                                    .WithIdentity(jobSchedule.TriggerName, jobSchedule.JobGroup)
                                    .WithSimpleSchedule(x => x.WithIntervalInHours(24).RepeatForever())
                                    .Build();
        }

        // Schedule the job
        await scheduler.ScheduleJob(jobDetail, trigger);
    }
}

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
