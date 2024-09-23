using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(string emailTo, string emailSubject, string emailBody,
            string? emailCc = null, string attachment = null, bool includeAlternateview = false);
    }
}
