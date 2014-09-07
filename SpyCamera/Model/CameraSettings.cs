// SpyCamera - CameraSettings.cs
// 
// Paweł Wróblewski
// 22:28 - 13.08.2014

using System;
using System.Xml.Serialization;

namespace SpyCamera.Model
{
    [Serializable]
    public class CameraSettings
    {
        [XmlElement]
        public string Directory { get; set; } 

        [XmlElement]
        public int Frames { get; set; }

        [XmlElement]
        public int MaxSizeMb { get; set; }
    }
}