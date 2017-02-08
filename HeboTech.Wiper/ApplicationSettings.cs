namespace HeboTech.Wiper
{
    public class ApplicationSettings : ISettings
    {
        public T GetSetting<T>(string name)
        {
            return (T)Properties.Settings.Default[name];
        }

        public void Save()
        {
            Properties.Settings.Default.Save();
        }

        public void SetSetting(string name, object value)
        {
            Properties.Settings.Default[name] = value;
        }
    }
}
