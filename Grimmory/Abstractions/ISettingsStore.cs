using Grimmory.Settings;

namespace Grimmory.Abstractions
{
    internal interface ISettingsStore
    {
        AppSettings Load();
        bool Save(AppSettings settings);
    }
}
