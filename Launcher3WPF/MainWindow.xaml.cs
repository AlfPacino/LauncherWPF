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
using System.Diagnostics;

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

        List<AppToLaunch> appManager = null;
        Button[] buttons = null;

        //method displays appManager in Window + displays 1 additional button "Add New App"
        private void ShowAllApps()
        {
            //Clear grid
            mainGrid.ColumnDefinitions.Clear();
            mainGrid.RowDefinitions.Clear();
            mainGrid.Children.Clear();
            buttons = null;

            // 3 columns
            mainGrid.ColumnDefinitions.Add(new ColumnDefinition());
            mainGrid.ColumnDefinitions.Add(new ColumnDefinition());
            mainGrid.ColumnDefinitions.Add(new ColumnDefinition());

            // add rows acording to count of apps
            for (int i = 0; i <= appManager.Count / 3; i++)
            {
                mainGrid.RowDefinitions.Add(new RowDefinition());
            }
            
            buttons = new Button[appManager.Count+1]; // +1 cause of button "Add new App"

            int currentCol = 0;
            int currentRow = 0;

            // this loop adds new button and fits the grid for it
            for (int i = 0; i < appManager.Count; i++)
            {
                buttons[i] = new Button();
                buttons[i].Name = "button" + i.ToString();

                System.Windows.Controls.Image buf = new System.Windows.Controls.Image();
                buf.Source = appManager[i].GetImage();
                //EllipseGeometry clipGeometry = new EllipseGeometry(new Point(buf.Source.Height/2, buf.Source.Width / 2), 20, 20);
                //buf.Clip = clipGeometry;
                buttons[i].Content = buf;
                

                buttons[i].MouseRightButtonUp += ButtonDelete_Click; // add event that deletes button 
                buttons[i].Click += LaunchApp; // add event that launches app
                buttons[i].ToolTip = appManager[i].exePath; // add hint
                
                buttons[i].Style = this.FindResource("MainButtonStyle") as Style; // add style to button
                
                if (currentCol == 3)
                {
                    currentCol = 0;
                    currentRow++;
                }

                mainGrid.Children.Add(buttons[i]);
                Grid.SetRow(buttons[i], currentRow);
                Grid.SetColumn(buttons[i], currentCol);

                currentCol++;
            }

            // this code adds 1 additional button wich alow to add new apps
            Button buttonAdd = new Button();
            buttonAdd.Name = "buttonAdd";
            buttonAdd.Content = "Add New App";
            buttonAdd.Click += ButtonAdd_Click;
            buttonAdd.Style = this.FindResource("MainButtonStyle") as Style;
            if (currentCol == 3)
            {
                currentCol = 0;
                currentRow++;
            }
            mainGrid.Children.Add(buttonAdd);
            Grid.SetColumn(buttonAdd, currentCol);
            Grid.SetRow(buttonAdd, currentRow);
        }

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

            // IF exe file not found - delete AppToLaunch linked with him
            foreach (AppToLaunch app in appManager)
            {
                if (!File.Exists(app.exePath))
                {
                    appManager.Remove(app);
                }
            }

            ShowAllApps();
        }

        private void ButtonAdd_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog();
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (openFileDialog.FileName.EndsWith(".exe"))
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
                        MessageBox.Show("Application has already been added");
                    }

                    ShowAllApps();

                    Serealization();
                }
                else
                {
                    MessageBox.Show("Selected file is not .exe");
                }
            }
        }

        private void ButtonDelete_Click(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            // delete app 
            int i = Convert.ToInt32(b.Name.Remove(0, 6)); // first 6 symbols of name are "button" then goes number
            appManager.RemoveAt(i);
            buttons = null;
            ShowAllApps();
            // serealisation to xml
            Serealization();
        }

        private void LaunchApp(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            int i = Convert.ToInt32(b.Name.Remove(0, 6)); // first 6 symbols of name are "button" then goes number
            try
            {
                Process.Start(appManager[i].exePath); // launch chosen app
            }
            catch{ return; }
            Environment.Exit(0);
        }

        private void Exit(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

    }
}
