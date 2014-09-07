// SpyCamera - ILogService.cs
// 
// Paweł Wróblewski
// 09:32 - 01.08.2014

using System;

namespace SpyCamera.Interfaces.LogService
{
    public interface ILogService
    {
        void Log(string message);
        void Log(Exception exception);
    }
}