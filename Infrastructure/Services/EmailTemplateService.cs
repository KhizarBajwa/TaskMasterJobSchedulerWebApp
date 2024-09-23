using Core.Interfaces.Services;
using Infrastructure.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class EmailTemplateService : IEmailTemplateService
    {
        public string GetTimesheetReminderEmailTemplate()
        {
            try
            {
                return File.ReadAllText($"{AppDomain.CurrentDomain.BaseDirectory}\\EmailTemplates\\TimesheetReminderEmailTemplate.html");

            }
            catch (Exception ex)
            {
                throw new InfrastructureException("Unable to read from Timesheet Reminder Email Template", ex);
            }
        }
    }
}
