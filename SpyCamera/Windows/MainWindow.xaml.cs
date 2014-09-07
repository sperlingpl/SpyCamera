using System.Windows;
using System.Windows.Controls;
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
        }

        private void OnOpenCameraRecordingSettings(CameraViewModel obj)
        {
            var cameraConfigWindow = new CameraRecordingSettings();
            Messenger.Default.Send(obj, WindowToken.OpenCameraRecordingSettings);
            cameraConfigWindow.Owner = this;
            cameraConfigWindow.ShowDialog();
        }

        private void AddCameraMenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            var newCameraWindow = new NewCameraWindow();
            newCameraWindow.Owner = this;
            newCameraWindow.ShowDialog();
        }

        private void CameraSettingsButton(object sender, RoutedEventArgs e)
        {
            Button btn = (Button) sender;
            if (btn.DataContext is CameraViewModel)
            {
                OnOpenCameraRecordingSettings((CameraViewModel)btn.DataContext);
            }
        }
    }
}