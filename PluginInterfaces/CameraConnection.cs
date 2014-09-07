// PluginInterfaces - CameraConnection.cs
// 
// Paweł Wróblewski
// 10:09 - 30.07.2014

using System;
using System.Xml.Serialization;

namespace PluginInterfaces
{
    [Serializable]
    public class CameraConnection
    {
        [XmlElement]
        public string Address { get; set; }

        [XmlElement]
        public string Login { get; set; }

        [XmlElement]
        public string Password { get; set; }
    }
}