using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class JobRunLog
    {
        [Key]
        public int JobRunId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int StatusId { get; set; }
        public string Server { get; set; }

        // We can say this is application id
        [ForeignKey("JobSchedule_JobId")]
        public int JobSchedule_JobId { get; set; }

        public JobSchedule JobSchedule { get; set; } // Navigation property
    }
}
