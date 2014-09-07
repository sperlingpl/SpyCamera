// SpyCamera - IPluginService.cs
// 
// Paweł Wróblewski
// 21:37 - 29.07.2014

using System.Collections.Generic;
using System.Threading.Tasks;
using PluginInterfaces;
using SpyCamera.Model;

namespace SpyCamera.Interfaces.PluginService
{
    public interface IPluginService
    {
        Task FindPlugins();
        List<PluginInfo> GetPluginList();
        PluginInfo GetPluginInfo(string fileName);
        ICameraPlugin CreatePluginInstance(PluginInfo pluginInfo);
    }
}