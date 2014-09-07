using System.Windows;
using System.Windows.Forms;
using GalaSoft.MvvmLight.Messaging;
using SpyCamera.Enums.Window;
using SpyCamera.Extensions;
using SpyCamera.ViewModel;

namespace SpyCamera.Windows
{
    /// <summary>
    ///     Interaction logic for CameraRecordingSettings.xaml
    /// </summary>
    public partial class CameraRecordingSettings : Window
    {
        public CameraRecordingSettings()
        {
            InitializeComponent();

            Messenger.Default.Register<object>(this, WindowToken.CloseCameraRecordingSettings, OnCloseSettings);
        }

        private void OnCloseSettings(object obj)
        {
            this.Close();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new FolderBrowserDialog();

            if (dialog.ShowDialog(this.GetWin32Window()) == System.Windows.Forms.DialogResult.OK)
            {
                ((CameraRecordingSettingsViewModel) DataContext).SaveDirectory = dialog.SelectedPath;
            }
        }
    }
}