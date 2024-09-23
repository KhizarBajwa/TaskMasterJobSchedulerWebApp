using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities.TimesheetReminder; // Adjust based on your namespaces
using Core.Interfaces.Repositories;
using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;

namespace Infrastructure.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly string _connectionStringDpsCommon;
        private readonly string _connectionStringLas;
        private readonly IConfiguration _configuration;

        public EmployeeRepository(IConfiguration configuration)
        {
            _configuration = configuration;

            _connectionStringDpsCommon = _configuration.GetConnectionString("DefaultConnection_DPSCommon");
            _connectionStringLas = _configuration.GetConnectionString("DefaultConnection_LAS");
        }

        // Retrieves all employees who are not supervisors
        public async Task<IEnumerable<Employee>> GetAllEmployeesAsync()
        {
            const string query = @"
            SELECT e.EmployeeID, e.EmployeeName
            FROM dps_common.dbo.Employee e
            LEFT JOIN SupervisorDelegate sd ON e.EmployeeID = sd.assignedby
            WHERE sd.assignedby IS NULL AND e.EmployeeDPS_Status = 'A';
        ";

            using (var connection = new SqlConnection(_connectionStringDpsCommon))
            {
                try
                {
                    return await connection.QueryAsync<Employee>(query);
                }
                catch (Exception ex)
                {
                    // Log the exception (use a logger if available)
                    throw new Exception("Error fetching employees.", ex);
                }
            }
        }

        // Retrieves all supervisors
        public async Task<IEnumerable<Supervisor>> GetAllSupervisorsAsync()
        {
            const string query = @"
            SELECT DISTINCT e.EmployeeID, e.EmployeeName, aduser.mail AS Email
            FROM LAS.dbo.SupervisorDelegate sup
            INNER JOIN dps_common.dbo.Employee e ON e.EmployeeID = sup.assignedBY 
            INNER JOIN dps_common.dbo.aduser aduser ON e.EmployeeID = aduser.PS_EmployeeID
            WHERE sup.assignedby IS NOT NULL AND e.EmployeeDPS_Status = 'A';
        ";

            using (var connection = new SqlConnection(_connectionStringLas))
            {
                try
                {
                    return await connection.QueryAsync<Supervisor>(query);
                }
                catch (Exception ex)
                {
                    // Log the exception (use a logger if available)
                    throw new Exception("Error fetching supervisors.", ex);
                }
            }
        }

        // Retrieves all subordinates for a supervisor
        public async Task<List<EmployeeSubOrdinatDto>> GetAllSupervisorsEmployeesAsync(string supervisorEmployeeId)
        {
            const string storedProcedure = "cp_GetEmpListing";
            var parameters = new
            {
                emplid = supervisorEmployeeId,
                startdate = DateTime.Now,
                endDate = DateTime.Now
            };

            using (var connection = new SqlConnection(_connectionStringLas))
            {
                try
                {
                    var employees = (await connection.QueryAsync<EmployeeSubOrdinatDto>(storedProcedure, parameters, commandType: System.Data.CommandType.StoredProcedure)).ToList();
                    return employees;
                }
                catch (Exception ex)
                {
                    // Log the exception (use a logger if available)
                    throw new Exception("Error fetching supervisor's employees.", ex);
                }
            }
        }

        // Retrieves an employee's email by their name
        public async Task<string> GetMailByEmployeeNameAsync(string employeeName)
        {
            const string query = @"
            SELECT ad.mail
            FROM dps_common.dbo.Employee e
            INNER JOIN dps_common.dbo.ADUser ad ON e.EmployeeID = ad.PS_EmployeeID
            WHERE e.EmployeeName = @EmployeeName;
        ";

            using (var connection = new SqlConnection(_connectionStringDpsCommon))
            {
                try
                {
                    return await connection.QueryFirstOrDefaultAsync<string>(query, new { EmployeeName = employeeName });
                }
                catch (Exception ex)
                {
                    // Log the exception (use a logger if available)
                    throw new Exception("Error fetching employee email.", ex);
                }
            }
        }

        // Retrieves employees who did not submit their timesheets last week
        public async Task<IEnumerable<Employee>> GetEmployeesWhoDidNotSubmitTimesheetLastWeekAsync()
        {
            const string query = @"
            DECLARE @LastWeekStart DATE, @LastWeekEnd DATE;
            SET @LastWeekStart = DATEADD(WEEK, DATEDIFF(WEEK, 0, GETDATE()) - 1, 1);
            SET @LastWeekEnd = DATEADD(DAY, 6, @LastWeekStart);

            WITH AllEmployees AS (
                SELECT DISTINCT e.EmployeeID, e.EmployeeName, ad.mail
                FROM dps_common.dbo.Employee e
                INNER JOIN DPSPortal.dbo.OE_EmploymentApproval ea ON e.EmployeeID = ea.employeeID
                LEFT JOIN dps_common.dbo.ADUser ad ON e.EmployeeID = ad.PS_EmployeeID
                WHERE e.EmployeeDPS_Status = 'A'
            ),
            SubmittedLastWeek AS (
                SELECT DISTINCT EmployeeID
                FROM DPSPortal.dbo.ODTS_Hours
                WHERE SubmittedDate >= @LastWeekStart AND SubmittedDate < DATEADD(DAY, 1, @LastWeekEnd)
            )
            SELECT e.EmployeeID, e.EmployeeName, e.mail AS Email
            FROM AllEmployees e
            LEFT JOIN SubmittedLastWeek s ON e.EmployeeID = s.EmployeeID
            WHERE s.EmployeeID IS NULL
            ORDER BY e.EmployeeName;
        ";

            using (var connection = new SqlConnection(_connectionStringDpsCommon))
            {
                try
                {
                    return await connection.QueryAsync<Employee>(query);
                }
                catch (Exception ex)
                {
                    // Log the exception (use a logger if available)
                    throw new Exception("Error fetching employees who did not submit timesheets.", ex);
                }
            }
        }
    }
}
