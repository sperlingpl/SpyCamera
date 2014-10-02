using System;
using System.IO;
using System.Windows;
using GalaSoft.MvvmLight.Threading;

namespace SpyCamera
{
    /// <summary>
    ///     Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            DispatcherHelper.Initialize();

            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            StreamWriter file = new StreamWriter("error_log.txt");
            file.WriteLine(e.ExceptionObject.ToString());
            file.Close();
            
            Environment.Exit(1);
        }
    }
}