using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.IO;

namespace Launcher3WPF
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        AppManager appManager;

        private void Window_Loaded(object sender, EventArgs e)
        {
            try
            {
                if (File.Exists("Config.xml"))
                {
                    // deserealisation
                    using (Stream stream = new FileStream("Config.xml", FileMode.Open))
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(AppManager));
                        appManager = (AppManager)serializer.Deserialize(stream);
                    }
                }
                else
                {
                    appManager = new AppManager(0);
                }
            }
            catch (Exception exep)
            {
                appManager = new AppManager(0);
                System.Windows.MessageBox.Show("Error while decoding Config.xml: " + Environment.NewLine + exep.Message);
            }
        }

        private void ButtonAdd_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if(openFileDialog.FileName.EndsWith(".exe"))
                {
                    // add new app
                    BitmapImage bmp = new BitmapImage(new Uri("C:\\Users\\Dex\\Desktop\\кот.jpg"));


                    // serealisation data to xml
                    using (Stream writer = new FileStream("Config.xml", FileMode.Create))
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(AppManager));
                        serializer.Serialize(writer, appManager);
                    }
                }
            }
        }

        private void ButtonDelete_Click(object sender, EventArgs e)
        {
            // delete app 

            // serealisation to xml
            using (Stream writer = new FileStream("Config.xml", FileMode.Create))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(AppManager));
                serializer.Serialize(writer, appManager);
            }
        }

        private void ButtonExit_Click(object sender, EventArgs e)
        { 
            Environment.Exit(0);
        }

    }
}
