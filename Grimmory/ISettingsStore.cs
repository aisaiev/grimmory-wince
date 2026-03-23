namespace Grimmory
{
    internal interface ISettingsStore
    {
        AppSettings Load();
        bool Save(AppSettings settings);
    }
}
