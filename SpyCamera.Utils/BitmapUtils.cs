using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;
using Point = System.Drawing.Point;

namespace SpyCamera.Utils
{
    public class BitmapUtils
    {
        public static BitmapImage BitmapToBitmapImage(Bitmap bitmap)
        {
            var bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();

            var ms = new MemoryStream();
            bitmap.Save(ms, ImageFormat.Bmp);
            ms.Seek(0, SeekOrigin.Begin);

            bitmapImage.StreamSource = ms;
            bitmapImage.EndInit();
            bitmapImage.Freeze();

            return bitmapImage;
        }

        public static Bitmap BitmapImageToBitmap(BitmapImage source)
        {
            var bmp = new Bitmap(source.PixelWidth, source.PixelHeight, PixelFormat.Format32bppRgb);
            BitmapData data = bmp.LockBits(new Rectangle(Point.Empty, bmp.Size), ImageLockMode.WriteOnly,
                PixelFormat.Format32bppRgb);

            source.CopyPixels(Int32Rect.Empty, data.Scan0, data.Height*data.Stride, data.Stride);
            bmp.UnlockBits(data);

            return bmp;
        }
    }
}