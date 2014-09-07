// SpyCamera - ISettingsService.cs
// 
// Paweł Wróblewski
// 11:49 - 31.07.2014

namespace SpyCamera.Interfaces.SettingsService
{
    public interface ISettingsService
    {
        void LoadSettings();
        void SaveSettings();
    }
}