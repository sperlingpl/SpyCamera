using System.Windows;
using GalaSoft.MvvmLight.Messaging;
using SpyCamera.Enums.Window;
using SpyCamera.ViewModel;
using SpyCamera.Windows;

namespace SpyCamera.Windows
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Messenger.Default.Register<CameraViewModel>(this, WindowToken.OpenCameraRecordingSettings, OnOpenCameraRecordingSettings);
        }

        private void OnOpenCameraRecordingSettings(CameraViewModel obj)
        {
            var cameraConfigWindow = new CameraRecordingSettings();
            cameraConfigWindow.Owner = this;
            cameraConfigWindow.ShowDialog();
        }

        private void AddCameraMenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            var newCameraWindow = new NewCameraWindow();
            newCameraWindow.Owner = this;
            newCameraWindow.ShowDialog();
        }
    }
}