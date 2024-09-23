using Core.Entities.TimesheetReminder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.Repositories
{
    public interface IEmployeeRepository
    {
        Task<IEnumerable<Employee>> GetAllEmployeesAsync();
        Task<IEnumerable<Supervisor>> GetAllSupervisorsAsync();
        Task<List<EmployeeSubOrdinatDto>> GetAllSupervisorsEmployeesAsync(string supervisorEmployeeId);
        Task<string> GetMailByEmployeeNameAsync(string employeeName);
        Task<IEnumerable<Employee>> GetEmployeesWhoDidNotSubmitTimesheetLastWeekAsync();
    }

}
