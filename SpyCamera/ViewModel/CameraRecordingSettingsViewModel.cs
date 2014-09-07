// SpyCamera - CameraRecordingSettingsViewModel.cs
// 
// Paweł Wróblewski
// 11:52 - 01.08.2014

using System;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using SpyCamera.Enums.Window;
using SpyCamera.Interfaces.CameraService;
using SpyCamera.Interfaces.SettingsService;
using SpyCamera.Model;

namespace SpyCamera.ViewModel
{
    public class CameraRecordingSettingsViewModel : ViewModelBase
    {
        public ObservableCollection<int> FramesList { get; private set; }
        private readonly ICameraService cameraService;
        private readonly ISettingsService settingsService;

        private CameraSettings cameraSettings;

        #region SaveDirectory

        /// <summary>
        ///     The <see cref="SaveDirectory" /> property's name.
        /// </summary>
        public const string SaveDirectoryPropertyName = "SaveDirectory";

        /// <summary>
        ///     Sets and gets the SaveDirectory property.
        ///     Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public string SaveDirectory
        {
            get { return saveDirectory; }

            set
            {
                if (saveDirectory == value)
                {
                    return;
                }

                RaisePropertyChanging(SaveDirectoryPropertyName);
                saveDirectory = value;
                RaisePropertyChanged(SaveDirectoryPropertyName);
            }
        }

        private string saveDirectory = string.Empty;

        #endregion

        #region Frames

        /// <summary>
        ///     The <see cref="Frames" /> property's name.
        /// </summary>
        public const string FramesPropertyName = "Frames";

        /// <summary>
        ///     Sets and gets the Frames property.
        ///     Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public int Frames
        {
            get { return frames; }

            set
            {
                RaisePropertyChanging(FramesPropertyName);
                frames = value;
                RaisePropertyChanged(FramesPropertyName);
            }
        }

        private int frames = 1;

        #endregion

        #region MaxMbSize

        /// <summary>
        ///     The <see cref="MaxMbSize" /> property's name.
        /// </summary>
        public const string MaxMbSizePropertyName = "MaxMbSize";

        /// <summary>
        ///     Sets and gets the MaxMbSize property.
        ///     Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public int MaxMbSize
        {
            get { return maxMbSize; }

            set
            {
                if (maxMbSize == value)
                {
                    return;
                }

                RaisePropertyChanging(MaxMbSizePropertyName);
                maxMbSize = value;
                RaisePropertyChanged(MaxMbSizePropertyName);
            }
        }

        private int maxMbSize;

        #endregion

        public CameraRecordingSettingsViewModel(ISettingsService settingsService, ICameraService cameraService)
        {
            this.settingsService = settingsService;
            this.cameraService = cameraService;

            FramesList = new ObservableCollection<int>();

            MessengerInstance.Register<CameraViewModel>(this, WindowToken.OpenCameraRecordingSettings, OnOpenSettings);
        }

        private void OnOpenSettings(CameraViewModel camera)
        {
            if (camera == null)
                return;

            cameraSettings = camera.Camera.CameraSettings;

            FramesList.Clear();

            foreach (int fps in cameraService.GetFramesPerSecondList(camera.Camera))
            {
                FramesList.Add(fps);
            }

            SaveDirectory = cameraSettings.Directory;
            Frames = cameraSettings.Frames;
            MaxMbSize = cameraSettings.MaxSizeMb;
        }

        #region SaveCommand

        /// <summary>
        ///     Gets the SaveCommand.
        /// </summary>
        public RelayCommand SaveCommand
        {
            get
            {
                return saveCommand
                       ?? (saveCommand = new RelayCommand(ExecuteSaveCommand));
            }
        }

        private RelayCommand saveCommand;

        private void ExecuteSaveCommand()
        {
            cameraSettings.Directory = SaveDirectory;
            cameraSettings.Frames = Frames;
            cameraSettings.MaxSizeMb = MaxMbSize;

            settingsService.SaveSettings();

            MessengerInstance.Send(new Object(), WindowToken.CloseCameraRecordingSettings);
        }

        #endregion
    }
}