using System;
using System.Collections.ObjectModel;
using System.Linq;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Threading;
using PluginInterfaces;
using SpyCamera.Enums.Camera;
using SpyCamera.Enums.Window;
using SpyCamera.Interfaces.CameraService;
using SpyCamera.Interfaces.PluginService;
using SpyCamera.Interfaces.SettingsService;
using SpyCamera.Model;

namespace SpyCamera.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        public ObservableCollection<CameraViewModel> Cameras { get; private set; }
        public ObservableCollection<Camera> CameraImages { get; private set; } 

        #region CameraAddress

        /// <summary>
        ///     The <see cref="CameraAddress" /> property's name.
        /// </summary>
        public const string CameraAddressPropertyName = "CameraAddress";

        /// <summary>
        ///     Sets and gets the CameraAddress property.
        ///     Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public string CameraAddress
        {
            get { return cameraAddress; }

            set
            {
                if (cameraAddress == value)
                {
                    return;
                }

                RaisePropertyChanging(CameraAddressPropertyName);
                cameraAddress = value;
                RaisePropertyChanged(CameraAddressPropertyName);
            }
        }

        private string cameraAddress = string.Empty;

        #endregion

        #region SelectedCamera

        /// <summary>
        ///     The <see cref="SelectedCamera" /> property's name.
        /// </summary>
        public const string SelectedCameraPropertyName = "SelectedCamera";

        /// <summary>
        ///     Sets and gets the SelectedCamera property.
        ///     Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public CameraViewModel SelectedCamera
        {
            get { return selectedCamera; }

            set
            {
                RaisePropertyChanging(SelectedCameraPropertyName);
                selectedCamera = value;
                RaisePropertyChanged(SelectedCameraPropertyName);
            }
        }

        private CameraViewModel selectedCamera;

        #endregion

        /// <summary>
        /// The <see cref="CameraOne" /> property's name.
        /// </summary>
        public const string CameraOnePropertyName = "CameraOne";

        private Camera cameraOne = null;

        /// <summary>
        /// Sets and gets the CameraOne property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public Camera CameraOne
        {
            get
            {
                return cameraOne;
            }

            set
            {
                RaisePropertyChanging(CameraOnePropertyName);
                cameraOne = value;
                RaisePropertyChanged(CameraOnePropertyName);
            }
        }

        private readonly ICameraService cameraService;
        private IPluginService pluginService;

        public MainViewModel(ICameraService cameraService, IPluginService pluginService, ISettingsService settingsService)
        {
            this.cameraService = cameraService;
            this.pluginService = pluginService;

            MessengerInstance.Register<Camera>(this, CameraMessengerToken.NewFrame, OnNewFrameReceived);
            MessengerInstance.Register<Camera>(this, CameraMessengerToken.StatusChange, OnCameraStatusChange);
            MessengerInstance.Register<object>(this, WindowToken.RefreshCameraList, OnRefreshCameraList);

            Cameras = new ObservableCollection<CameraViewModel>();
            CameraImages = new ObservableCollection<Camera>();

            init();

            settingsService.LoadSettings();

            OnRefreshCameraList(null);
        }

        private void OnRefreshCameraList(object obj)
        {
            Cameras.Clear();

            foreach (Camera camera in cameraService.GetCameraList())
                Cameras.Add(new CameraViewModel(camera));
        }

        private async void init()
        {
            await pluginService.FindPlugins();

            //var camera = new Camera { Address = "http://128.104.138.26/axis-cgi/mjpg/video.cgi", Name = "kamerka" };
            //var camera1 = new Camera { Address = "http://192.168.0.128", Name = "Dom", PluginInfo = pluginService.GetPluginList()[0] };
            //Cameras.Add(new CameraViewModel(camera));
            //Cameras.Add(new CameraViewModel(camera1));
        }

        private void OnCameraStatusChange(Camera camera)
        {
            Cameras.First(x => x.Camera == camera).ChangeStatus();
        }

        private void OnNewFrameReceived(Camera camera)
        {
            DispatcherHelper.CheckBeginInvokeOnUI(() =>
            {
                CameraOne = camera;

                //if (SelectedCamera.Camera == camera) { }
                //SelectedCamera.Camera = camera;
            });
        }

        #region StartCaptureCommand

        /// <summary>
        ///     Gets the StartCaptureCommand.
        /// </summary>
        public RelayCommand StartCaptureCommand
        {
            get
            {
                return startCaptureCommand
                       ?? (startCaptureCommand = new RelayCommand(ExecuteStartCaptureCommand));
            }
        }

        private RelayCommand startCaptureCommand;

        private void ExecuteStartCaptureCommand()
        {
            cameraService.StartCaptureMjpegStream(SelectedCamera.Camera);
        }

        #endregion

        #region StopCameraCommand

        /// <summary>
        ///     Gets the StopCaptureCommand.
        /// </summary>
        public RelayCommand StopCaptureCommand
        {
            get
            {
                return stopCaptureCommand
                       ?? (stopCaptureCommand = new RelayCommand(ExecuteStopCaptureCommand));
            }
        }

        private RelayCommand stopCaptureCommand;

        private void ExecuteStopCaptureCommand()
        {
            cameraService.StopCaptureMjpegStream(SelectedCamera.Camera);
        }

        #endregion

        private RelayCommand cameraConfigCommand;

        /// <summary>
        /// Gets the CameraConfigCommand.
        /// </summary>
        public RelayCommand CameraConfigCommand
        {
            get
            {
                return cameraConfigCommand
                    ?? (cameraConfigCommand = new RelayCommand(ExecuteCameraConfigCommand));
            }
        }

        private void ExecuteCameraConfigCommand()
        {
            MessengerInstance.Send(SelectedCamera, WindowToken.OpenCameraRecordingSettings);
        }
    }
}