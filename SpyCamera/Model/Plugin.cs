// SpyCamera - Plugin.cs
// 
// Paweł Wróblewski
// 10:56 - 30.07.2014

using System;
using PluginInterfaces;

namespace SpyCamera.Model
{
    public class Plugin
    {
        public PluginInfo PluginInfo { get; set; }
        public Type PluginType { get; set; }
    }
}