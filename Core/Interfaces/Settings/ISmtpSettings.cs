using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.Settings
{
    public interface ISmtpSettings
    {
        public string SmtpHost { get;  }
        public string SmtpPort { get;  }
        public string SmtpUserName { get;  }
        public string SmtpPassword { get; }
        public string SmtpEmailFrom { get;  }
        public string SmtpEmailTo { get;  }
        public string SmtpEmailCc { get;  }
    }
}
