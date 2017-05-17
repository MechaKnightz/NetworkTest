using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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
using BespokeFusion;
using Library;
using Newtonsoft.Json;
using Path = System.IO.Path;

namespace ServerGUI
{
    //MaterialMessageBox.Show("Your cool message here", "The awesome message title");
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Task _task;
        private Server _server;
        private CancellationTokenSource _cancellationTokenSource;
        public World World;
        public LoggerManager LoggerManager;
        private string _filePath { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            LoggerManager = new LoggerManager();

            World = new World();
            LoggerManager = new LoggerManager();
            _server = new Server(LoggerManager, World);

            DataContext = LoggerManager;

            var _lock = new object();
            BindingOperations.EnableCollectionSynchronization(LoggerManager.LogMessages, _lock);
        }

        private void BtnStart_Click(object sender, RoutedEventArgs e)
        {
            BtnStart.IsEnabled = false;
            BtnStop.IsEnabled = true;
            LoggerManager.AddLogMessage("Server", "Starting...");
            {
                try
                {
                    var saveString = File.ReadAllText(_filePath);

                    World.Circles = JsonConvert.DeserializeObject<List<Circle>>(saveString);

                }
                catch (Exception ex)
                {
                    if (ex is DirectoryNotFoundException)
                        MaterialMessageBox.Show("Invalid directory\nException: " + ex.Message);
                    else if (ex is FileNotFoundException)
                        MaterialMessageBox.Show("Invalid file name\nException: " + ex.Message);
                    else if(ex is ArgumentNullException)
                        MaterialMessageBox.Show("You must select a map file first\nException: " + ex.Message);
                    else
                        MaterialMessageBox.Show("Error.\nException: " + ex.Message);
                    LoggerManager.AddServerLogMessage("Could not load map file, Shutting down...");
                    BtnStart.IsEnabled = true;
                    BtnStop.IsEnabled = false;
                    return; //TODO return
                }
                LoggerManager.AddServerLogMessage("Successfully imported map.");
            }
            _cancellationTokenSource = new CancellationTokenSource();
            _task = new Task(_server.Run, _cancellationTokenSource.Token);
            _task.Start();
            World = _server.World;
        }

        private void BtnStop_Click(object sender, RoutedEventArgs e)
        {
            if (_task != null && _cancellationTokenSource != null)
            {
                LoggerManager.AddServerLogMessage("Stopping server...");
                _cancellationTokenSource.Cancel();
                BtnStart.IsEnabled = true;
                BtnStop.IsEnabled = false;
            }
        }
        //not mine
        private void control_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            // If the entire contents fit on the screen, ignore this event
            if (e.ExtentHeight < e.ViewportHeight)
                return;

            // If no items are available to display, ignore this event
            if (ConsoleDataGrid.Items.Count <= 0)
                return;

            // If the ExtentHeight and ViewportHeight haven't changed, ignore this event
            if (e.ExtentHeightChange == 0.0 && e.ViewportHeightChange == 0.0)
                return;

            // If we were close to the bottom when a new item appeared,
            // scroll the new item into view.  We pick a threshold of 5
            // items since issues were seen when resizing the window with
            // smaller threshold values.
            var oldExtentHeight = e.ExtentHeight - e.ExtentHeightChange;
            var oldVerticalOffset = e.VerticalOffset - e.VerticalChange;
            var oldViewportHeight = e.ViewportHeight - e.ViewportHeightChange;
            if (oldVerticalOffset + oldViewportHeight + 2 >= oldExtentHeight)
                ConsoleDataGrid.ScrollIntoView(ConsoleDataGrid.Items[ConsoleDataGrid.Items.Count - 1]);
        }

        private void BtnSelectFile_Click(object sender, RoutedEventArgs e)
        {
            // Create OpenFileDialog 
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            string destPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\MapMaker\Saves\");

            Directory.CreateDirectory(destPath);
            // Set filter for file extension and default file extension 
            dlg.DefaultExt = ".json";
            dlg.InitialDirectory = destPath;
            //dlg.Filter = "JPEG Files (*.jpeg)|*.jpeg|PNG Files (*.png)|*.png|JPG Files (*.jpg)|*.jpg|GIF Files (*.gif)|*.gif";

            // Display OpenFileDialog by calling ShowDialog method 
            Nullable<bool> result = dlg.ShowDialog();

            // Get the selected file name and display in a TextBox 
            if (result == true)
            {
                _filePath = dlg.FileName;
                string filename = dlg.SafeFileName;
                LblFile.Content = filename;
            }
        }
    }
}
