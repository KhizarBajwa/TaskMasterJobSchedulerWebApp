using Core.Entities.TimesheetReminder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.Services
{
    public interface IEmailFormatterService
    {
        string GeneratedTimesheetReminderEmailBody(string supervisorName, List<Employee> employees);

    }
}
