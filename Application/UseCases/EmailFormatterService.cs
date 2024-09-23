using Core.Entities.TimesheetReminder;
using Core.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases
{
    public class EmailFormatterService : IEmailFormatterService
    {
             
        private readonly IEmailTemplateService _emailTemplateService;

        public EmailFormatterService(IEmailTemplateService emailTemplateService)
        {
            _emailTemplateService = emailTemplateService;
        }
        public string GeneratedTimesheetReminderEmailBody(string supervisorName, List<Employee> employees)
        {
            // Load the HTML template from the IEmailTemplateService
            var template = _emailTemplateService.GetTimesheetReminderEmailTemplate();

            // Replace placeholders in the template
            var emailBody = template.Replace("{SupervisorName}", supervisorName);

            // Generate the employee rows for the table
            var employeeRows = new StringBuilder();
            foreach (var employee in employees)
            {
                employeeRows.Append($@"
                <tr>
                    <td>{employee.EmployeeID}</td>
                    <td>{employee.EmployeeName}</td>
                    <td>{employee.Email}</td>
                </tr>");
            }

            // Replace the {EmployeeRows} placeholder with the generated rows
            emailBody = emailBody.Replace("{EmployeeRows}", employeeRows.ToString());

            // Replace {YourCompanyName} with a placeholder or hard-coded company name
            emailBody = emailBody.Replace("{YourCompanyName}", "DPS");

            return emailBody;
        }
    }
}
