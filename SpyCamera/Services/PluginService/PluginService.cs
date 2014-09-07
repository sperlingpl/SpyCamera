// SpyCamera - PluginService.cs
// 
// Paweł Wróblewski
// 21:37 - 29.07.2014

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using PluginInterfaces;
using SpyCamera.Interfaces.PluginService;
using SpyCamera.Model;

namespace SpyCamera.Services.PluginService
{
    public class PluginService : IPluginService
    {
        private const string PluginDirectory = "Plugins";

        private readonly List<Plugin> plugins;

        public PluginService()
        {
            plugins = new List<Plugin>();
        }

        public async Task FindPlugins()
        {
            await Task.Run(() =>
            {
                if (!Directory.Exists(PluginDirectory))
                    return;

                foreach (string file in Directory.GetFiles(PluginDirectory))
                {
                    var fileInfo = new FileInfo(file);

                    if (fileInfo.Extension == ".dll")
                    {
                        LoadPlugin(fileInfo);
                    }
                }
            });
        }

        public ICameraPlugin CreatePluginInstance(PluginInfo pluginInfo)
        {
            return (ICameraPlugin) Activator.CreateInstance(plugins.First(x => x.PluginInfo == pluginInfo).PluginType);
        }

        public List<PluginInfo> GetPluginList()
        {
            return plugins.Select(plugin => plugin.PluginInfo).ToList();
        }

        public PluginInfo GetPluginInfo(string fileName)
        {
            Plugin plugin = plugins.FirstOrDefault(x => x.PluginInfo.FileName == fileName);

            if (plugin != null)
                return plugin.PluginInfo;
            else
                return null;
        }

        private void LoadPlugin(FileInfo fileInfo)
        {
            Assembly pluginAssembly = Assembly.LoadFrom(fileInfo.FullName);

            foreach (Type type in pluginAssembly.GetExportedTypes())
            {
                if (type.GetInterface(typeof (ICameraPlugin).Name) != null)
                {
                    var pluginInstance = (ICameraPlugin) Activator.CreateInstance(type);
                    var plugin = new Plugin {PluginType = type, PluginInfo = pluginInstance.GetPluginInfo()};

                    plugin.PluginInfo.FileName = fileInfo.Name;

                    plugins.Add(plugin);
                }
            }
        }
    }
}