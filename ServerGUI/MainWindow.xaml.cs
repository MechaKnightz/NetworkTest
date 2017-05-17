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
            LoggerManager.AddLogMessage("Server", "Starting...");
            //MaterialMessageBox.Show("Your cool message here", "The awesome message title");
            var mapNameInput = "map1";
            while (true)
            {
                try
                {

                    string mapName = StringCheck.MakeValidFileName(mapNameInput);


                    string destPath = Path.Combine(
                        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\MapMaker\Saves\",
                        mapName + ".json");

                    var saveString = File.ReadAllText(destPath);

                    World.Circles = JsonConvert.DeserializeObject<List<Circle>>(saveString);

                }
                catch (Exception ex)
                {
                    if (ex is DirectoryNotFoundException)
                        LoggerManager.AddServerLogMessage("Invalid directoty: " + ex.Message);
                    else if (ex is FileNotFoundException)
                        LoggerManager.AddServerLogMessage("Invalid file name: " + ex.Message);
                    else LoggerManager.AddServerLogMessage("Error: " + ex.Message);

                    LoggerManager.AddServerLogMessage("Could not load map file.");
                    continue; //TODO return
                }
                LoggerManager.AddServerLogMessage("Successfully imported map.");

                break;
            }
            BtnStart.IsEnabled = false;
            BtnStop.IsEnabled = true;

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
    }
}
