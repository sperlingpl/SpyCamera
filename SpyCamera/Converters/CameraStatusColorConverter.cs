// SpyCamera - CameraStatusColorConverter.cs
// 
// Paweł Wróblewski
// 11:45 - 28.07.2014

using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using PluginInterfaces.Enums;
using SpyCamera.Enums.Camera;

namespace SpyCamera.Converters
{
    public class CameraStatusColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var cameraStatus = (CameraStatus) value;

            Brush textColor;

            switch (cameraStatus)
            {
                case CameraStatus.Stopped:
                    textColor = Brushes.OrangeRed;
                    break;

                case CameraStatus.Error:
                    textColor = Brushes.Red;
                    break;

                case CameraStatus.Capturing:
                    textColor = Brushes.Green;
                    break;

                case CameraStatus.Loading:
                    textColor = Brushes.CornflowerBlue;
                    break;

                case CameraStatus.Recording:
                    textColor = Brushes.Peru;
                    break;

                default:
                    textColor = Brushes.Black;
                    break;
            }

            return textColor;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}