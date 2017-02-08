namespace HeboTech.Wiper
{
    public interface ISettings
    {
        T GetSetting<T>(string name);
        void SetSetting(string name, object value);
        void Save();
    }
}
