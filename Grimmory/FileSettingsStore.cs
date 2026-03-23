namespace Grimmory
{
    internal sealed class FileSettingsStore : ISettingsStore
    {
        public AppSettings Load()
        {
            return AppSettings.Load();
        }

        public bool Save(AppSettings settings)
        {
            return settings != null && settings.Save();
        }
    }
}
