using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class JobSchedule
    {
        [Key]
        public int JobId { get; set; }
        public string JobName { get; set; }
        public string JobGroup { get; set; }
        public string Description { get; set; }
        public string NumberOfThreads { get; set; }
        // This expression means run every 30 seconds "0/30 * * * * ?"
        public string CronTriggerExpression { get; set; }
        public string TriggerType { get; set; } // Cron, Daily, Simple
        public string TriggerName { get; set; }
        public bool IsCancelled { get; set; } // New property
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }

}
