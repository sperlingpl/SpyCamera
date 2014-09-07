// SpyCamera - CameraDevice.cs
// 
// Paweł Wróblewski
// 12:21 - 30.07.2014

using System;
using System.IO;
using System.Windows.Media.Imaging;
using AForge.Video.FFMPEG;
using PluginInterfaces;
using SpyCamera.Model;
using SpyCamera.Utils;

namespace SpyCamera.Services.CameraService
{
    internal class CameraDevice
    {
        public Camera Camera { get; set; }
        public ICameraPlugin CameraPlugin { get; set; }

        private readonly VideoFileWriter fileWriter;
        private bool isRecording;

        private string recordingFilePath;

        public CameraDevice()
        {
            fileWriter = new VideoFileWriter();
            isRecording = false;
        }

        public void OpenForWrite()
        {
            isRecording = true;
        }

        public void Close()
        {
            fileWriter.Close();

            isRecording = false;
        }

        public void AddFrame(BitmapImage bitmapImage)
        {
            if (!isRecording)
                return;

            if (!fileWriter.IsOpen)
            {
                recordingFilePath = Camera.CameraSettings.Directory;

                string fileName = string.Format("{0}{1:_dd.MM.yyyy_hh.mm.ss}.avi", Camera.Name, DateTime.Now);

                recordingFilePath = Path.Combine(recordingFilePath, fileName);

                fileWriter.Open(recordingFilePath, bitmapImage.PixelWidth, bitmapImage.PixelHeight,
                    CameraPlugin.GetFramerate(),
                    VideoCodec.MPEG4);
            }

            fileWriter.WriteVideoFrame(BitmapUtils.BitmapImageToBitmap(bitmapImage));

            var fileInfo = new FileInfo(recordingFilePath);

            if (fileInfo.Length >= (Camera.CameraSettings.MaxSizeMb * 1024 * 1024))
            {
                fileWriter.Close();
            }
        }
    }
}