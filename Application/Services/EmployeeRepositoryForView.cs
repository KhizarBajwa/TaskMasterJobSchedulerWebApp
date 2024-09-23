using Application.ViewModels;
using Core.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class EmployeeRepositoryForView
    {
        private readonly IEmployeeRepository _employeeRepository;

        public EmployeeRepositoryForView(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        // Retrieves the defaulters grouped by their supervisors
        public async Task<List<SupervisorViewModel>> GetDefaultersGroupedBySupervisorsAsync()
        {
            // Fetch employees who have not submitted their timesheets last week
            var employeesWhoDidNotSubmitTimesheet = await _employeeRepository.GetEmployeesWhoDidNotSubmitTimesheetLastWeekAsync();

            // Fetch all supervisors
            var allSupervisors = await _employeeRepository.GetAllSupervisorsAsync();

            var supervisorViewModels = new List<SupervisorViewModel>();

            // Iterate through the supervisors and check their subordinates
            foreach (var supervisor in allSupervisors)
            {
                var subordinates = await _employeeRepository.GetAllSupervisorsEmployeesAsync(supervisor.EmployeeID);
                var subordinateEmployeeIDs = subordinates.Select(e => e.Emplid).ToList();

                // Filter employees who are defaulters under this supervisor
                var defaulters = employeesWhoDidNotSubmitTimesheet
                                    .Where(emp => subordinateEmployeeIDs.Contains(emp.EmployeeID))
                                    .ToList();

                if (defaulters.Any())
                {
                    // Add to the list if defaulters exist under this supervisor
                    supervisorViewModels.Add(new SupervisorViewModel
                    {
                        SupervisorName = supervisor.EmployeeName,
                        SupervisorEmail = supervisor.Email,
                        Defaulters = defaulters.Select(emp => new DefaulterViewModel
                        {
                            EmployeeName = emp.EmployeeName
                        }).ToList()
                    });
                }
            }

            return supervisorViewModels;
        }
    }
}
