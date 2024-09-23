using Core.Interfaces.Settings;
using Infrastructure.Exceptions;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class AppSettingsRepository : IAppSettingsRepository
    {
        public readonly IConfiguration _configuration;

        public AppSettingsRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // Existing method to get boolean value
        public bool GetBooleanValue(string appSettingName)
        {
            if (_configuration[appSettingName] == null)
            {
                throw new ConfigurationMissingException($"Appsettings key is not defined: {appSettingName}");
            }
            try
            {
                return _configuration.GetValue<bool>(appSettingName);
            }
            catch (Exception ex)
            {
                throw new ConfigurationMissingException($"Unable to read the config value for the key {appSettingName}", ex);
            }
        }

        // Get integer value from app settings
        public int GetIntValue(string appSettingName)
        {
            if (_configuration[appSettingName] == null)
            {
                throw new ConfigurationMissingException($"Appsettings key is not defined: {appSettingName}");
            }
            try
            {
                return _configuration.GetValue<int>(appSettingName);
            }
            catch (Exception ex)
            {
                throw new ConfigurationMissingException($"Unable to read the config value for the key {appSettingName}", ex);
            }
        }

        // Get a strongly-typed section from appsettings.json
        public T GetSection<T>(string sectionName)
        {
            if (_configuration.GetSection(sectionName) == null)
            {
                throw new ConfigurationMissingException($"Appsettings section is not defined: {sectionName}");
            }
            try
            {
                return _configuration.GetSection(sectionName).Get<T>();
            }
            catch (Exception ex)
            {
                throw new ConfigurationMissingException($"Unable to read the config section for {sectionName}", ex);
            }
        }

        // Get a list of values from a section in appsettings.json
        public IEnumerable<T> GetSectionValues<T>(string sectionName)
        {
            var section = _configuration.GetSection(sectionName);
            if (section == null || !section.Exists())
            {
                throw new ConfigurationMissingException($"Appsettings section is not defined: {sectionName}");
            }
            try
            {
                return section.Get<IEnumerable<T>>();
            }
            catch (Exception ex)
            {
                throw new ConfigurationMissingException($"Unable to read the config values for section {sectionName}", ex);
            }
        }

        // Get a secure string value from appsettings.json
        public SecureString GetSecureStringValue(string appSettingName)
        {
            if (_configuration[appSettingName] == null)
            {
                throw new ConfigurationMissingException($"Appsettings key is not defined: {appSettingName}");
            }
            try
            {
                var value = _configuration.GetValue<string>(appSettingName);
                var secureString = new SecureString();
                foreach (char c in value)
                {
                    secureString.AppendChar(c);
                }
                secureString.MakeReadOnly();
                return secureString;
            }
            catch (Exception ex)
            {
                throw new ConfigurationMissingException($"Unable to read the secure string value for the key {appSettingName}", ex);
            }
        }

        // Get a list of string values from appsettings.json
        public IEnumerable<string> GetStringList(string appSettingName)
        {
            if (_configuration.GetSection(appSettingName) == null)
            {
                throw new ConfigurationMissingException($"Appsettings key is not defined: {appSettingName}");
            }
            try
            {
                return _configuration.GetSection(appSettingName).Get<IEnumerable<string>>();
            }
            catch (Exception ex)
            {
                throw new ConfigurationMissingException($"Unable to read the string list for the key {appSettingName}", ex);
            }
        }

        // Get a string value from appsettings.json
        public string GetStringValue(string appSettingName)
        {
            if (_configuration[appSettingName] == null)
            {
                throw new ConfigurationMissingException($"Appsettings key is not defined: {appSettingName}");
            }
            try
            {
                return _configuration.GetValue<string>(appSettingName);
            }
            catch (Exception ex)
            {
                throw new ConfigurationMissingException($"Unable to read the config value for the key {appSettingName}", ex);
            }
        }

    }

}
