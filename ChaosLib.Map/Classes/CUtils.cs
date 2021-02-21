using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace ChaosLib.Map.Classes
{
    public class CUtils
    {
        public static Bitmap CreateBitmap(int w, int h, byte[] pd)
        {
            Bitmap bmp = new Bitmap(w, h, PixelFormat.Format32bppArgb);
            BitmapData data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.WriteOnly, bmp.PixelFormat);
            Marshal.Copy(pd, byte.MinValue, data.Scan0, pd.Length);
            bmp.UnlockBits(data);

            return bmp;
        }
    }
}
