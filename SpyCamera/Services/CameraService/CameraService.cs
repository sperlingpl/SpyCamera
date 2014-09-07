using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media.Imaging;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;
using PluginInterfaces;
using PluginInterfaces.Enums;
using SpyCamera.Enums.Camera;
using SpyCamera.Interfaces.CameraService;
using SpyCamera.Interfaces.PluginService;
using SpyCamera.Model;

namespace SpyCamera.Services.CameraService
{
    public class CameraService : ICameraService
    {
        private readonly List<CameraDevice> cameraList;

        public CameraService()
        {
            cameraList = new List<CameraDevice>();
        }

        public void AddCamera(Camera camera)
        {
            var pluginService = SimpleIoc.Default.GetInstance<IPluginService>();
            ICameraPlugin cameraPlugin = pluginService.CreatePluginInstance(camera.PluginInfo);

            cameraPlugin.OnVideoFrame += cameraPlugin_OnVideoFrame;
            cameraPlugin.OnError += cameraPlugin_OnError;
            cameraPlugin.OnStatusChanged += cameraPlugin_OnStatusChanged;

            var cameraDevice = new CameraDevice() { Camera = camera, CameraPlugin = cameraPlugin };
            cameraList.Add(cameraDevice);
        }

        public void RemoveCamera(Camera camera)
        {
            var cameraDevice = GetCamera(camera);

            if (cameraDevice != null)
                cameraList.Remove(cameraDevice);
        }

        public IEnumerable<Camera> GetCameraList()
        {
            return cameraList.Select(camera => camera.Camera).ToList();
        }

        public void StartCaptureMjpegStream(Camera camera)
        {
            CameraDevice cameraDevice = cameraList.FirstOrDefault(x => x.Camera == camera);
            ICameraPlugin cameraPlugin = null;

            if (cameraDevice == null)
            {
                var pluginService = SimpleIoc.Default.GetInstance<IPluginService>();
                cameraPlugin = pluginService.CreatePluginInstance(camera.PluginInfo);

                cameraPlugin.OnVideoFrame += cameraPlugin_OnVideoFrame;
                cameraPlugin.OnError += cameraPlugin_OnError;
                cameraPlugin.OnStatusChanged += cameraPlugin_OnStatusChanged;

                cameraDevice = new CameraDevice() {Camera = camera, CameraPlugin = cameraPlugin};
                cameraList.Add(cameraDevice);
            }

            cameraPlugin = cameraDevice.CameraPlugin;
            
            cameraPlugin.SetFramerate(camera.CameraSettings.Frames);
            cameraPlugin.StartVideoCapture(camera.CameraConnection);
            StartRecordingVideo(camera); // temp
        }

        public void StartRecordingVideo(Camera camera)
        {
            cameraList.First(x=>x.Camera == camera).OpenForWrite();
        }

        public void StopRecordingVideo(Camera camera)
        {
            cameraList.First(x => x.Camera == camera).Close();
        }

        public IEnumerable<int> GetFramesPerSecondList(Camera camera)
        {
            return GetCamera(camera).CameraPlugin.GetAllowedFramerateList();
        }

        public void StopCaptureMjpegStream(Camera camera)
        {
            StopRecordingVideo(camera); // temp
            cameraList.First(x => x.Camera == camera).CameraPlugin.StopVideoCapture();
        }

        private void cameraPlugin_OnStatusChanged(object sender, CameraStatus e)
        {
            Camera cameraPlugin = cameraList.First(x => x.CameraPlugin == sender).Camera;
            cameraPlugin.Status = e;
            Messenger.Default.Send(cameraPlugin, CameraMessengerToken.StatusChange);
        }

        private void cameraPlugin_OnError(object sender, EventArgs e)
        {
            Camera cameraPlugin = cameraList.First(x => x.CameraPlugin == sender).Camera;
            cameraPlugin.Status = CameraStatus.Error;
            Messenger.Default.Send(cameraPlugin, CameraMessengerToken.StatusChange);
        }

        private void cameraPlugin_OnVideoFrame(object sender, BitmapImage e)
        {
            CameraDevice cameraDevice = cameraList.First(x => x.CameraPlugin == sender);
            cameraDevice.Camera.Image = e;
            cameraDevice.AddFrame(e);

            Messenger.Default.Send(cameraDevice.Camera, CameraMessengerToken.NewFrame);
        }

        private CameraDevice GetCamera(Camera camera)
        {
            return cameraList.FirstOrDefault(x => x.Camera == camera);
        }
    }
}