// SpyCamera - SettingsService.cs
// 
// Paweł Wróblewski
// 11:50 - 31.07.2014

using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using GalaSoft.MvvmLight.Ioc;
using SpyCamera.Interfaces.CameraService;
using SpyCamera.Interfaces.PluginService;
using SpyCamera.Interfaces.SettingsService;
using SpyCamera.Model;

namespace SpyCamera.Services.SettingsService
{
    public class SettingsService : ISettingsService
    {
        private readonly ICameraService cameraService;
        private readonly IPluginService pluginService;

        /// <summary>
        ///     Path with settings filename.
        /// </summary>
        private readonly string settingsFilePath;

        /// <summary>
        ///     Path to current user settings folder.
        /// </summary>
        private readonly string settingsPath;

        /// <summary>
        /// Settings directory name.
        /// </summary>
        private const string SettingsDirectoryName = "SpyCamera";

        private Settings settings;

        public SettingsService()
        {
            settings = new Settings();
            cameraService = SimpleIoc.Default.GetInstance<ICameraService>();
            pluginService = SimpleIoc.Default.GetInstance<IPluginService>();

            settingsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), SettingsDirectoryName);

            if (!Directory.Exists(settingsPath))
                Directory.CreateDirectory(settingsPath);

            settingsFilePath = Path.Combine(settingsPath, "settings.xml");
        }

        public void LoadSettings()
        {
            try
            {
                var serializer = new XmlSerializer(typeof (Settings));

                TextReader reader = new StreamReader(settingsFilePath);
                settings = (Settings) serializer.Deserialize(reader);
                reader.Close();

                foreach (Camera camera in settings.Cameras)
                {
                    camera.PluginInfo = pluginService.GetPluginInfo(camera.PluginInfo.FileName);

                    if (camera.CameraSettings == null)
                        camera.CameraSettings = new CameraSettings();

                    cameraService.AddCamera(camera);
                }
            }
            catch (FileNotFoundException)
            {
            }
            catch (DirectoryNotFoundException)
            {
            }
            catch (IOException)
            {
            }
        }

        public async void SaveSettings()
        {
            settings.Cameras = new List<Camera>(cameraService.GetCameraList());

            var serializer = new XmlSerializer(typeof (Settings));
            TextWriter writer = new StreamWriter(settingsFilePath);
            serializer.Serialize(writer, settings);
            await writer.FlushAsync();
            writer.Close();
        }
    }
}