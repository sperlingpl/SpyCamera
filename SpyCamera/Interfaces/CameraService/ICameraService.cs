using System.Collections.Generic;
using System.Windows.Documents;
using SpyCamera.Model;

namespace SpyCamera.Interfaces.CameraService
{
    public interface ICameraService
    {
        void AddCamera(Camera camera);
        void RemoveCamera(Camera camera);
        IEnumerable<Camera> GetCameraList();
        void StartCaptureMjpegStream(Camera camera);
        void StopCaptureMjpegStream(Camera camera);
        void StartRecordingVideo(Camera camera);
        void StopRecordingVideo(Camera camera);
        IEnumerable<int> GetFramesPerSecondList(Camera camera);
    }
}