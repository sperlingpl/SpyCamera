// PluginInterfaces - PluginInfo.cs
// 
// Paweł Wróblewski
// 21:31 - 29.07.2014

using System;
using System.Xml.Serialization;

namespace PluginInterfaces
{
    [Serializable]
    public class PluginInfo
    {
        [XmlIgnore]
        public string Author { get; set; }

        [XmlIgnore]
        public string Version { get; set; }

        [XmlIgnore]
        public string Name { get; set; }

        [XmlIgnore]
        public bool NeedAuthorization { get; set; }

        [XmlElement]
        public string FileName { get; set; }
    }
}