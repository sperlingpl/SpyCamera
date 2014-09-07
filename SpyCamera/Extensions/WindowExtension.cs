// SpyCamera - WindowExtension.cs
// 
// Paweł Wróblewski
// 21:52 - 13.08.2014

using System;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using IWin32Window = System.Windows.Forms.IWin32Window;

namespace SpyCamera.Extensions
{
    public static class WindowExtension
    {
        public static IWin32Window GetWin32Window(this Visual visual)
        {
            var source = PresentationSource.FromVisual(visual) as HwndSource;
            IWin32Window win32Window = new OldWindow(source.Handle);
            return win32Window;
        }

        private class OldWindow : IWin32Window
        {
            public IntPtr Handle { get; private set; }

            public OldWindow(IntPtr handlePtr)
            {
                Handle = handlePtr;
            }
        }
    }
}