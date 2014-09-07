// SpyCamera - Settings.cs
// 
// Paweł Wróblewski
// 12:05 - 31.07.2014

using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using SpyCamera.Model;

namespace SpyCamera.Services.SettingsService
{
    [Serializable]
    public class Settings
    {
        [XmlArray]
        public List<Camera> Cameras { get; set; }

        public Settings()
        {
            Cameras = new List<Camera>();
        }
    }
}