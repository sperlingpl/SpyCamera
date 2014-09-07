// PluginInterfaces - IPlugin.cs
// 
// Paweł Wróblewski
// 10:25 - 29.07.2014

using System;
using System.Collections.Generic;
using System.Windows.Media.Imaging;
using PluginInterfaces.Enums;

namespace PluginInterfaces
{
    public interface ICameraPlugin
    {
        PluginInfo GetPluginInfo();

        void StartVideoCapture(CameraConnection cameraConnection);
        void StopVideoCapture();
        void SetFramerate(int fps);
        List<int> GetAllowedFramerateList(); 
        int GetFramerate();

        event EventHandler OnError;
        event EventHandler<BitmapImage> OnVideoFrame;
        event EventHandler<CameraStatus> OnStatusChanged;
    }
}