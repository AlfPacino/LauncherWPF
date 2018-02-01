using System.IO;
using System.Windows.Media.Imaging;
using System.Xml.Serialization;

namespace Launcher3WPF
{
    public struct AppToLaunch
    {
        public AppToLaunch(string exePath, BitmapSource bmpImage)
        {
            this.exePath = exePath;

            // convert BitmapImage => byte array
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
            }
        }

        // converts byte array to BitmapImage
        public BitmapImage GetImage()
        {
            var image = new BitmapImage();
            using (var mem = new MemoryStream(bmpBytes))
            {
                mem.Position = 0;
                image.BeginInit();
                image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.UriSource = null;
                image.StreamSource = mem;
                image.EndInit();
            }
            image.Freeze();
            return image;
        }

        [XmlElement("Image")]
        public byte[] bmpBytes;

        public string exePath; 
    }
}
