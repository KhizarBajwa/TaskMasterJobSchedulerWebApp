using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities.TimesheetReminder
{
    public class EmployeeSubOrdinatDto
    {
        public string Emplid { get; set; }
        public string Employeename { get; set; }
        public string ReadyForReview { get; set; }
        public string SupApproved { get; set; }
        public DateTime sDate { get; set; }
        public DateTime edate { get; set; }
        public string UploadStatus { get; set; }
        public string TSStatus { get; set; }
    }
}
