using Screenshot.Settings.API;

namespace SelectArea.Common;

public class SettingsAPI
{
    public SettingsHelper.Settings Settings { get; private set; }
    
    public SettingsAPI()
    {
        Settings = SettingsHelper.getOrCreateGeneralSettings();
    }

    public void UpdateSettings(SettingsHelper.Settings settings)
    {
        SettingsHelper.updateGeneralSettings(settings);
        Settings = SettingsHelper.getOrCreateGeneralSettings();
    }

    public SettingsHelper.Settings Clone()
    {
        return new SettingsHelper.Settings(Settings.hotkeySelectArea);
    }
}