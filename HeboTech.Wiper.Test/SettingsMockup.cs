using System.Collections.Generic;

namespace HeboTech.Wiper.Test
{
    public class SettingsMockup : ISettings
    {
        private IDictionary<string, object> settings;

        public SettingsMockup(IDictionary<string, object> settings)
        {
            this.settings = settings;
        }

        public T GetSetting<T>(string name)
        {
            return (T)settings[name];
        }

        public void Save()
        {
            //Do nothing.
        }

        public void SetSetting(string name, object value)
        {
            settings[name] = value;
        }
    }
}
