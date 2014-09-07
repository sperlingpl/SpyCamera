/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocator xmlns:vm="clr-namespace:SpyCamera"
                           x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"

  You can also use Blend to do all this with the tool's support.
  See http://www.galasoft.ch/mvvm
*/

using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using SpyCamera.Interfaces.CameraService;
using SpyCamera.Interfaces.PluginService;
using SpyCamera.Interfaces.SettingsService;
using SpyCamera.Services.CameraService;
using SpyCamera.Services.PluginService;
using SpyCamera.Services.SettingsService;

namespace SpyCamera.ViewModel
{
    /// <summary>
    ///     This class contains static references to all the view models in the
    ///     application and provides an entry point for the bindings.
    /// </summary>
    public class ViewModelLocator
    {
        public MainViewModel Main
        {
            get { return ServiceLocator.Current.GetInstance<MainViewModel>(); }
        }

        public AddCameraViewModel AddCamera
        {
            get { return SimpleIoc.Default.GetInstance<AddCameraViewModel>(); }
        }

        public CameraRecordingSettingsViewModel CameraRecordingSettings
        {
            get { return SimpleIoc.Default.GetInstance<CameraRecordingSettingsViewModel>(); }
        }

        /// <summary>
        ///     Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            ////if (ViewModelBase.IsInDesignModeStatic)
            ////{
            ////    // Create design time view services and models
            ////    SimpleIoc.Default.Register<IDataService, DesignDataService>();
            ////}
            ////else
            ////{
            ////    // Create run time view services and models
            ////    SimpleIoc.Default.Register<IDataService, DataService>();
            ////}

            SimpleIoc.Default.Register<ICameraService>(() => new CameraService());
            SimpleIoc.Default.Register<IPluginService>(() => new PluginService());
            SimpleIoc.Default.Register<ISettingsService>(() => new SettingsService());

            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<AddCameraViewModel>();
            SimpleIoc.Default.Register<CameraRecordingSettingsViewModel>(true);
        }

        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}