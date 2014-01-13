using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace MvcApplication2
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }

        private static bool isInited;
        public override void Init()
        {
            if (!isInited)
            {
                var files = Directory.GetFiles(@"c:\users\sht3ch\documents\visual studio 2013\Projects\MvcApplication2\MvcApplication2\pics\Full").Where(name => name.EndsWith(".jpg"));
                foreach (var file in files)
                {
                    var old = new Bitmap(file);
                    var New = ScaleByWidthAndHeight(old, 150, 100, 50);
                    New.Save(@"c:\users\sht3ch\documents\visual studio 2013\Projects\MvcApplication2\MvcApplication2\pics\minis\" + Path.GetFileName(file));
                }
                foreach (var file in files)
                {
                    var old = new Bitmap(file);
                    var New = ScaleByWidthAndHeight(old, 400, 200, 50);
                    New.Save(@"c:\users\sht3ch\documents\visual studio 2013\Projects\MvcApplication2\MvcApplication2\pics\mediums\" + Path.GetFileName(file));
                }
            }
            isInited = true;
            base.Init();
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