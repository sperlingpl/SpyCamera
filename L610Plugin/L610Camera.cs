using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Media.Imaging;
using AForge.Video;
using PluginInterfaces;
using PluginInterfaces.Enums;
using SpyCamera.Utils;

namespace L610Plugin
{
    public class L610Camera : ICameraPlugin
    {
        private IVideoSource videoSource;
        private int fps;

        public L610Camera()
        {
            fps = 25;
        }

        public PluginInfo GetPluginInfo()
        {
            var pluginInfo = new PluginInfo
            {
                Author = "Paweł Wróblewski",
                Version = "1.0",
                Name = "L 610",
                NeedAuthorization = true
            };
            
            return pluginInfo;
        }

        public void StartVideoCapture(CameraConnection cameraConnection)
        {
            StopVideoCapture();

            OnStatusChanged(this, CameraStatus.Loading);

            videoSource = new MJPEGStream(CreateAddressToCameraCapture(cameraConnection));
            videoSource.NewFrame += videoSource_NewFrame;
            videoSource.VideoSourceError += videoSource_VideoSourceError;

            videoSource.Start();
        }

        public void StopVideoCapture()
        {
            if (videoSource != null)
            {
                videoSource.SignalToStop();

                // Wait ~3 seconds.
                for (int i = 0; i < 30; i++)
                {
                    if (!videoSource.IsRunning)
                        break;

                    Thread.Sleep(100);
                }

                if (videoSource.IsRunning)
                    videoSource.Stop();

                videoSource = null;

                OnStatusChanged(this, CameraStatus.Stopped);
            }
        }

        public event EventHandler OnError;
        public event EventHandler<BitmapImage> OnVideoFrame;
        public event EventHandler<CameraStatus> OnStatusChanged;

        private string CreateAddressToCameraCapture(CameraConnection cameraConnection)
        {
            string address;

            if (cameraConnection.Address.EndsWith("/"))
                address = cameraConnection.Address;
            else
                address = cameraConnection.Address + "/";

            address = string.Format("{0}videostream.cgi?user={1}&pwd={2}&resolution=32", address, cameraConnection.Login,
                cameraConnection.Password);

            return address;
        }

        public void SetFramerate(int fps)
        {
            this.fps = fps;
        }

        public List<int> GetAllowedFramerateList()
        {
            return new List<int>() {5, 25, 30};
        }

        public int GetFramerate()
        {
            return fps;
        }

        private void videoSource_VideoSourceError(object sender, VideoSourceErrorEventArgs eventargs)
        {
            OnStatusChanged(this, CameraStatus.Error);
            videoSource.Stop();
        }

        private void videoSource_NewFrame(object sender, NewFrameEventArgs eventargs)
        {
            OnStatusChanged(this, CameraStatus.Capturing);

            OnVideoFrame(this, BitmapUtils.BitmapToBitmapImage(eventargs.Frame));
        }
    }
}