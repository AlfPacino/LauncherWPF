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
            this.Loaded += new RoutedEventHandler(MainWindow_Loaded); // add loaded event to mainForm
            this.Closed += new EventHandler(MainWindow_Closed); // add closed event to mainForm
        }

        private void Serealization()
        {
            // serealisation data to xml
            using (Stream writer = new FileStream("Config.xml", FileMode.Create))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<AppToLaunch>));
                serializer.Serialize(writer, appManager);
            }
        }

        private void Deserealization()
        {
            using (Stream stream = new FileStream("Config.xml", FileMode.Open))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<AppToLaunch>));
                appManager = (List<AppToLaunch>)serializer.Deserialize(stream);
            }
        }

        List<AppToLaunch> appManager;

        private void MainWindow_Loaded(object sender, EventArgs e)
        {
            try
            {
                if (File.Exists("Config.xml"))
                {
                    Deserealization();
                }
                else
                {
                    appManager = new List<AppToLaunch>();
                }
            }
            catch (Exception exep)
            {
                appManager = new List<AppToLaunch>();
                System.Windows.MessageBox.Show("Error while decoding Config.xml: " + Environment.NewLine + exep.Message);
            }
            System.Windows.MessageBox.Show("Count " + appManager.Count);
            System.Windows.Controls.Image buf = new System.Windows.Controls.Image();
            buf.Source = appManager[0].GetImage();
            buttonAdd.Content = buf;
            mainGrid.RowDefinitions.Add(new RowDefinition());
        }

        private void ButtonAdd_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog();
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if(openFileDialog.FileName.EndsWith(".exe"))
                {                   
                    // get bmp from .exe
                    System.Drawing.Icon ico = System.Drawing.Icon.ExtractAssociatedIcon(openFileDialog.FileName);
                    BitmapSource bmp = System.Windows.Interop.Imaging.CreateBitmapSourceFromHIcon(
                                  ico.Handle,
                                  System.Windows.Int32Rect.Empty,
                                  System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions());

                    AppToLaunch newApp = new AppToLaunch(openFileDialog.FileName, bmp);

                    if (!appManager.Contains(newApp)) // DOENT WORK
                    {
                        appManager.Add(newApp);
                    }
                    else
                    {
                        System.Windows.MessageBox.Show("Application has already been added");
                    }
                    
                    Serealization();                  
                }
            }
        }

        private void ButtonDelete_Click(object sender, EventArgs e)
        {
            // delete app 
            int i = 1;
            appManager.RemoveAt(i);
            // serealisation to xml
            Serealization();
        }

        private void ButtonExit_Click(object sender, EventArgs e)
        { 
            Environment.Exit(0);
        }

        private void MainWindow_Closed(object sender, EventArgs e)
        {
            //
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            Button btn = new Button();
            mainGrid.Children.Add(btn);
            Button btn2 = new Button();
            mainGrid.Children.Add(btn2);
        }
    }
}
