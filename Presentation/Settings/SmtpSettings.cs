using Core.Interfaces.Settings;
using Infrastructure;

namespace Presentation.Settings
{
    public class SmtpSettings : ISmtpSettings
    {
        public readonly IAppSettingsRepository _appSettings;

        public SmtpSettings(IAppSettingsRepository appSettings)
        {
            _appSettings = appSettings;
        }

        // Retrieve SmtpHost from the app settings
        public string SmtpHost => _appSettings.GetStringValue($"{nameof(SmtpSettings)}:{nameof(SmtpHost)}");

        // Retrieve SmtpPort from the app settings and convert it to an integer
        public string SmtpPort
        {
            get
            {
                var port = _appSettings.GetIntValue($"{nameof(SmtpSettings)}:{nameof(SmtpPort)}");
                return port.ToString();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        // Retrieve SmtpUserName from the app settings
        public string SmtpUserName
        {
            get => _appSettings.GetStringValue($"{nameof(SmtpSettings)}:{nameof(SmtpUserName)}");
            set => throw new NotImplementedException();
        }

        // Retrieve SmtpPassword (preferably stored securely)
        public string SmtpPassword
        {
            get
            {
                var securePassword = _appSettings.GetSecureStringValue($"{nameof(SmtpSettings)}:{nameof(SmtpPassword)}");
                // Convert SecureString to plain text (only if necessary for use)
                return new System.Net.NetworkCredential(string.Empty, securePassword).Password;
            }
            set => throw new NotImplementedException();
        }

        // Retrieve the From Email address
        public string SmtpEmailFrom
        {
            get => _appSettings.GetStringValue($"{nameof(SmtpSettings)}:{nameof(SmtpEmailFrom)}");
            set => throw new NotImplementedException();
        }

        // Retrieve the To Email address
        public string SmtpEmailTo
        {
            get => _appSettings.GetStringValue($"{nameof(SmtpSettings)}:{nameof(SmtpEmailTo)}");
            set => throw new NotImplementedException();
        }

        // Retrieve the Cc Email address (optional)
        public string SmtpEmailCc
        {
            get => _appSettings.GetStringValue($"{nameof(SmtpSettings)}:{nameof(SmtpEmailCc)}");
            set => throw new NotImplementedException();
        }
    }

}
