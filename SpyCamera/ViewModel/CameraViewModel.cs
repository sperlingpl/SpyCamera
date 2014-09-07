// SpyCamera - CameraViewModel.cs
// 
// Paweł Wróblewski
// 11:33 - 28.07.2014

using GalaSoft.MvvmLight;
using PluginInterfaces.Enums;
using SpyCamera.Enums.Camera;
using SpyCamera.Model;

namespace SpyCamera.ViewModel
{
    public class CameraViewModel : ViewModelBase
    {
        public Camera Camera { get; private set; }

        #region Name

        /// <summary>
        ///     The <see cref="Name" /> property's name.
        /// </summary>
        public const string NamePropertyName = "Name";

        /// <summary>
        ///     Sets and gets the Name property.
        ///     Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public string Name
        {
            get { return name; }

            set
            {
                if (name == value)
                {
                    return;
                }

                RaisePropertyChanging(NamePropertyName);
                name = value;
                RaisePropertyChanged(NamePropertyName);
            }
        }

        private string name = string.Empty;

        #endregion

        #region Status

        /// <summary>
        /// The <see cref="Status" /> property's name.
        /// </summary>
        public const string StatusPropertyName = "Status";

        private string status = string.Empty;

        /// <summary>
        /// Sets and gets the Status property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string Status
        {
            get { return status; }

            set
            {
                if (status == value)
                {
                    return;
                }

                RaisePropertyChanging(StatusPropertyName);
                status = value;
                RaisePropertyChanged(StatusPropertyName);
            }
        }

        #endregion

        #region StatusType

        /// <summary>
        /// The <see cref="StatusType" /> property's name.
        /// </summary>
        public const string StatusTypePropertyName = "StatusType";

        private CameraStatus statusType = CameraStatus.Stopped;

        /// <summary>
        /// Sets and gets the StatusType property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public CameraStatus StatusType
        {
            get { return statusType; }

            set
            {
                if (statusType == value)
                {
                    return;
                }

                RaisePropertyChanging(StatusTypePropertyName);
                statusType = value;
                RaisePropertyChanged(StatusTypePropertyName);
            }
        }

        #endregion


        public CameraViewModel(Camera camera)
        {
            Camera = camera;
            Name = camera.Name;
            Status = "Zatrzymane";
        }

        public void ChangeStatus()
        {
            StatusType = Camera.Status;

            switch (Camera.Status)
            {
                case CameraStatus.Stopped:
                    Status = "Zatrzymana";
                    break;

                case CameraStatus.Recording:
                    Status = "Nagrywanie";
                    break;

                case CameraStatus.Error:
                    Status = "Błąd";
                    break;

                case CameraStatus.Loading:
                    Status = "Łączenie...";
                    break;

                case CameraStatus.Capturing:
                    Status = "Uruchomiona";
                    break;
            }
        }
    }
}