using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Launcher3WPF
{
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
    }

    public struct AppToLaunch
    {
        public AppToLaunch(string exePath, BitmapImage bmpIcon)
        {
            this.exePath = exePath;
            this.bmpIcon = bmpIcon;
        }
        public string exePath;
        public BitmapImage bmpIcon;
    }
}
