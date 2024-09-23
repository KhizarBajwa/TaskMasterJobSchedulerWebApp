using Core.Entities.TimesheetReminder;
using Core.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases
{
    public class NotificationService : INotificationService
    {
        private readonly IEmailFormatterService _emailFormatterService;
        private readonly IEmailService _emailService;

        public NotificationService(IEmailFormatterService emailFormatterService, IEmailService emailService)
        {
            _emailFormatterService = emailFormatterService;
            _emailService = emailService;
        }

        public async void SendTimeSheetReminderEmail(int jobRunId)
        {
            try
            {
                // Retrieve the environment variable for the environment (e.g., Development, Production)
                var environment = Environment.GetEnvironmentVariable("DOTNET_Environment");
                var emailSubject = $"{environment?.ToUpperInvariant()} : Email Reminder Service";

                // Simulate getting the list of employees who haven't submitted their timesheet
                var employees = GetEmployeesWhoHaventSubmittedTimesheet(); // This method needs implementation

                // Generate the email body using the email formatter
                var emailBody = _emailFormatterService.GeneratedTimesheetReminderEmailBody("Supervisor Name", employees);

                // Supervisor's email address
                string supervisorEmail = "supervisor@example.com"; // Retrieve dynamically if needed

                // Send the email using the EmailService
                await _emailService.SendEmailAsync(supervisorEmail, emailSubject, emailBody);

                // Optionally, log the success of sending the email
                Console.WriteLine($"Email reminder sent successfully to {supervisorEmail}");
            }
            catch (Exception ex)
            {
                // Log the error or rethrow the exception
                Console.WriteLine($"Error sending timesheet reminder email: {ex.Message}");
                throw;
            }
        }

        // This is a placeholder for retrieving the list of employees who haven't submitted timesheets
        private List<Employee> GetEmployeesWhoHaventSubmittedTimesheet()
        {
            // Fetch employees from database or service, for now, returning a static list
            return new List<Employee>
        {
            new Employee { EmployeeID = "E001", EmployeeName = "John Doe", Email = "john@example.com" },
            new Employee { EmployeeID = "E002", EmployeeName = "Jane Smith", Email = "jane@example.com" }
        };
        }
    }
}
