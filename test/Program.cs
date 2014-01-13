using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace test
{
    class Program
    {
        static void Main(string[] args)
        {

            var old = new Bitmap(@"c:\users\sht3ch\documents\visual studio 2013\Projects\MvcApplication2\MvcApplication2\pics\Lenovo Jetpack - Original.jpg");
            var New = ScaleByWidthAndHeight(old, 150, 100, 50);
            New.Save(@"c:\users\sht3ch\documents\visual studio 2013\Projects\MvcApplication2\MvcApplication2\pics\mymini.jpg");
        }
        public static Bitmap ScaleByWidthAndHeight(Bitmap inStream, int maxWidth, int maxHeight, int resolutionDPI)
        {
            var originalBitmap = new Bitmap(inStream);
            double ratioWidthToHeight = originalBitmap.Width / (double)originalBitmap.Height;
            int newHeight;
            int newWidth;
            if (ratioWidthToHeight > 1)
            {
                newWidth = maxWidth;
                newHeight = (int)(newWidth / ratioWidthToHeight);
            }
            else
            {
                newHeight = maxHeight;
                newWidth = (int)(newHeight * ratioWidthToHeight);
            }
            Bitmap newBitmap = new Bitmap(newWidth, newHeight);
            Graphics graphic = Graphics.FromImage(newBitmap);
            graphic.InterpolationMode = InterpolationMode.HighQualityBicubic;
            graphic.DrawImage(originalBitmap, 0, 0, newBitmap.Width, newBitmap.Height);

            newBitmap.SetResolution(resolutionDPI, resolutionDPI);
            return newBitmap;
        }
    }
}
