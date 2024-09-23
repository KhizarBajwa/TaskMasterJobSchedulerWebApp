using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities.TimesheetReminder
{
    public class ExecutionLog
    {
        [Key]
        public long LogId { get; set; }
        public string RunInstanceId { get; set; } = "";
        public string LogType { get; set; } = "";
        public string JobName { get; set; } = "";
        public string JobGroup { get; set; } = "";
        public string TriggerName { get; set; } = "";
        public string TriggerGroup { get; set; } = "";
        public DateTimeOffset? ScheduleFireTimeUtc { get; set; }
        public DateTimeOffset FireTimeUtc { get; set; } = DateTime.Now;
        public TimeSpan JobRunTime { get; set; }
        public int RetryCount { get; set; } = 0;
        public string Result { get; set; } = "";
        public string ErrorMessage { get; set; } = "";
        public bool IsVetoed { get; set; } = false;
        public bool IsException { get; set; } = false;
        public bool IsSuccess { get; set; } = false;
        public string? ReturnCode { get; set; } = "";
        public DateTimeOffset DateAddedUtc { get; set; } = DateTime.Now;
    }
}
