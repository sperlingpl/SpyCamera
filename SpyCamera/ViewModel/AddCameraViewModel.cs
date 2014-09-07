using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using PluginInterfaces;
using SpyCamera.Enums.Window;
using SpyCamera.Interfaces.CameraService;
using SpyCamera.Interfaces.PluginService;
using SpyCamera.Interfaces.SettingsService;
using SpyCamera.Model;

namespace SpyCamera.ViewModel
{
    public class AddCameraViewModel : ViewModelBase
    {
        public ObservableCollection<PluginInfo> PluginsCollection { get; private set; }

        #region SelectedCameraType

        /// <summary>
        ///     The <see cref="SelectedCameraType" /> property's name.
        /// </summary>
        public const string SelectedCameraTypePropertyName = "SelectedCameraType";

        /// <summary>
        ///     Sets and gets the SelectedCameraType property.
        ///     Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public PluginInfo SelectedCameraType
        {
            get { return selectedCameraType; }

            set
            {
                if (selectedCameraType == value)
                {
                    return;
                }

                if ((selectedCameraType == null) || (selectedCameraType.NeedAuthorization != value.NeedAuthorization))
                {
                    MessengerInstance.Send(value.NeedAuthorization, WindowToken.AddCameraWindowAuthentication);
                }

                RaisePropertyChanging(SelectedCameraTypePropertyName);
                selectedCameraType = value;
                RaisePropertyChanged(SelectedCameraTypePropertyName);
            }
        }

        private PluginInfo selectedCameraType;

        #endregion

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

        #region Address

        /// <summary>
        ///     The <see cref="Address" /> property's name.
        /// </summary>
        public const string AddressPropertyName = "Address";

        /// <summary>
        ///     Sets and gets the Address property.
        ///     Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public string Address
        {
            get { return address; }

            set
            {
                if (address == value)
                {
                    return;
                }

                RaisePropertyChanging(AddressPropertyName);
                address = value;
                RaisePropertyChanged(AddressPropertyName);
            }
        }

        private string address = string.Empty;

        #endregion

        #region Login

        /// <summary>
        ///     The <see cref="Login" /> property's name.
        /// </summary>
        public const string LoginPropertyName = "Login";

        /// <summary>
        ///     Sets and gets the Login property.
        ///     Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public string Login
        {
            get { return login; }

            set
            {
                if (login == value)
                {
                    return;
                }

                RaisePropertyChanging(LoginPropertyName);
                login = value;
                RaisePropertyChanged(LoginPropertyName);
            }
        }

        private string login = string.Empty;

        #endregion

        #region Password

        /// <summary>
        ///     The <see cref="Password" /> property's name.
        /// </summary>
        public const string PasswordPropertyName = "Password";

        /// <summary>
        ///     Sets and gets the Password property.
        ///     Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public string Password
        {
            get { return password; }

            set
            {
                if (password == value)
                {
                    return;
                }

                RaisePropertyChanging(PasswordPropertyName);
                password = value;
                RaisePropertyChanged(PasswordPropertyName);
            }
        }

        private string password = string.Empty;

        #endregion

        private readonly ICameraService cameraService;
        private readonly ISettingsService settingsService;

        public AddCameraViewModel(IPluginService pluginService, ICameraService cameraService,
            ISettingsService settingsService)
        {
            this.cameraService = cameraService;
            this.settingsService = settingsService;

            PluginsCollection = new ObservableCollection<PluginInfo>(pluginService.GetPluginList());
        }

        public void SetFirstPluginElement()
        {
            if (PluginsCollection.Count > 0)
                SelectedCameraType = PluginsCollection[0];
        }

        #region ConfirmCommand

        /// <summary>
        ///     Gets the ConfirmCommand.
        /// </summary>
        public RelayCommand ConfirmCommand
        {
            get
            {
                return confirmCommand
                       ?? (confirmCommand = new RelayCommand(ExecuteConfirmCommand));
            }
        }

        private RelayCommand confirmCommand;

        private void ExecuteConfirmCommand()
        {
            var camera = new Camera
            {
                PluginInfo = SelectedCameraType,
                Name = Name,
                CameraSettings =  new CameraSettings(),
                CameraConnection = new CameraConnection
                {
                    Address = Address,
                    Login = Login,
                    Password = Password
                }
            };

            cameraService.AddCamera(camera);

            settingsService.SaveSettings();

            MessengerInstance.Send(new object(), WindowToken.RefreshCameraList);
            MessengerInstance.Send(new object(), WindowToken.CloseAddCameraWindow);
        }

        #endregion
    }
}