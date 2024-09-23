using Core.Interfaces.Services;
using Core.Interfaces.Settings;
using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

public class EmailService : IEmailService
{
    private readonly ISmtpSettings _smtpSettings;

    public EmailService(ISmtpSettings smtpSettings)
    {
        _smtpSettings = smtpSettings;
    }

    public async Task SendEmailAsync(string emailTo, string emailSubject, string emailBody, string? emailCc = null, string? attachment = null, bool includeAlternateview = false)
    {
        try
        {
            MailMessage message = new MailMessage
            {
                From = new MailAddress(_smtpSettings.SmtpEmailFrom),
                Subject = emailSubject,
                IsBodyHtml = true
            };

            // Add To recipients
            foreach (var address in emailTo.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries))
            {
                message.To.Add(address);
            }

            // Add CC recipients if provided
            if (!string.IsNullOrEmpty(emailCc))
            {
                foreach (var address in emailCc.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries))
                {
                    message.CC.Add(address);
                }
            }

            // Handle alternate view (text/plain and text/html)
            if (includeAlternateview)
            {
                var plainTextView = AlternateView.CreateAlternateViewFromString("This is the plain text version.", null, "text/plain");
                var htmlView = AlternateView.CreateAlternateViewFromString(emailBody, null, "text/html");

                message.AlternateViews.Add(plainTextView);
                message.AlternateViews.Add(htmlView);
            }
            else
            {
                message.Body = emailBody;
            }

            // Add attachment if provided
            if (!string.IsNullOrEmpty(attachment) && File.Exists(attachment))
            {
                message.Attachments.Add(new Attachment(attachment));
            }

            // Configure SmtpClient
            using (var client = new SmtpClient(_smtpSettings.SmtpHost, int.Parse(_smtpSettings.SmtpPort)))
            {
                client.Credentials = new NetworkCredential(_smtpSettings.SmtpUserName, _smtpSettings.SmtpPassword);
                client.EnableSsl = true; // Enabling SSL based on best practices; can be parameterized

                // Send the email asynchronously
                await client.SendMailAsync(message);
            }
        }
        catch (Exception ex)
        {
            // You can log or rethrow the exception as necessary
            Console.WriteLine($"Error sending email: {ex.Message}");
            throw;
        }
    }
}


