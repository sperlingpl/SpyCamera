using System;
using System.Windows.Media.Imaging;
using System.Xml.Serialization;
using PluginInterfaces;
using PluginInterfaces.Enums;

namespace SpyCamera.Model
{
    [Serializable]
    public class Camera
    {
        [XmlElement]
        public string Name { get; set; }

        [XmlElement]
        public CameraSettings CameraSettings { get; set; }

        [XmlElement]
        public CameraConnection CameraConnection { get; set; }

        [XmlIgnore]
        public BitmapImage Image { get; set; }

        [XmlIgnore]
        public CameraStatus Status { get; set; }

        [XmlElement]
        public PluginInfo PluginInfo { get; set; }

        public Camera()
        {
            Status = CameraStatus.Stopped;
        }
    }
}