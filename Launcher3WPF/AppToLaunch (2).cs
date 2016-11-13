using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Xml.Serialization;
using System.IO;
using System.Windows.Media;

namespace Launcher3WPF
{
    /*
    public class AppManager
    {
        public AppManager()
        {
            this.appCount = 0;
            apps = new List<AppToLaunch>();
        }
        public AppManager(int appCount)
        {
            this.appCount = appCount;
        }

        public int appCount;
        public List<AppToLaunch> apps;
        public void AddApp(string pathToExe, BitmapImage bmpIcon)
        {
            apps.Add(new AppToLaunch(pathToExe, bmpIcon));
            appCount++;
        }
    }
    */
    public struct AppToLaunch
    {
        public AppToLaunch(string exePath, BitmapSource bmpImage)
        {
            this.exePath = exePath;
            this.bmpImage = bmpImage;

            // convert BitmapSource => byte array
            this.bmpBytes = null;
            if (bmpImage != null)
            {
                using (var stream = new MemoryStream())
                {
                    var encoder = new PngBitmapEncoder(); 
                    encoder.Frames.Add(BitmapFrame.Create(bmpImage));
                    encoder.Save(stream);
                    this.bmpBytes = stream.ToArray();
                }
                // byte array => bitmapImage
                this.bmpImage = GetImage();
            }

        }

        // meth returns image BitmapImage from byte array
        public BitmapImage GetImage()
        {
            var png = new BitmapImage();
            using (var mem = new MemoryStream(bmpBytes))
            {
                mem.Position = 0;
                png.BeginInit();
                png.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                png.CacheOption = BitmapCacheOption.OnLoad;
                png.UriSource = null;
                png.StreamSource = mem;
                png.EndInit();
            }
            png.Freeze();
            return png;
        }
        
        [XmlIgnore]
        public BitmapSource bmpImage;
        
        [XmlElement("Image")]
        public byte[] bmpBytes;

        public string exePath; 
    }
}
