using System.Windows;
using System.Windows.Media.Animation;
using GalaSoft.MvvmLight.Messaging;
using SpyCamera.Enums.Window;
using SpyCamera.ViewModel;

namespace SpyCamera.Windows
{
    /// <summary>
    ///     Interaction logic for NewCameraWindow.xaml
    /// </summary>
    public partial class NewCameraWindow : Window
    {
        private Storyboard showLoginInfoStoryboard;
        private Storyboard hideLoginInfoStoryboard;

        public NewCameraWindow()
        {
            InitializeComponent();

            showLoginInfoStoryboard = FindResource("ShowLoginInfo") as Storyboard;
            hideLoginInfoStoryboard = FindResource("HideLoginInfo") as Storyboard;

            Messenger.Default.Register<bool>(this, WindowToken.AddCameraWindowAuthentication, OnCameraAuthentication);
            Messenger.Default.Register<object>(this, WindowToken.CloseAddCameraWindow, OnCloseWindow);

            ((AddCameraViewModel)DataContext).SetFirstPluginElement();
        }

        private void OnCloseWindow(object obj)
        {
            Close();
        }

        private void OnCameraAuthentication(bool needAuth)
        {
            if (needAuth)
                showLoginInfoStoryboard.Begin();
            else
                hideLoginInfoStoryboard.Begin();
        }

        private void CancelButton_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void PasswordBox_OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            ((AddCameraViewModel) DataContext).Password = passwordBox.Password;
        }
    }
}