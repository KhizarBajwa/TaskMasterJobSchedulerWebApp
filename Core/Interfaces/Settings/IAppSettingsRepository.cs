using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.Settings
{
    public interface IAppSettingsRepository
    {
        string GetStringValue(string appSettingName);
        bool GetBooleanValue(string appSettingName);
        int GetIntValue(string appSettingName);
        SecureString GetSecureStringValue(string appSettingName);
        T GetSection<T>(string sectionName);
        IEnumerable<string> GetStringList(string appSettingName);
        public IEnumerable<T> GetSectionValues<T>(string sectionName);
    }
}
