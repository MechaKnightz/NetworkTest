using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using BespokeFusion;
using Library;
using MahApps.Metro.Controls;
using Newtonsoft.Json;
using Path = System.IO.Path;
using System.Windows.Threading;
using ServerGUI.Commands;
using ServerGUI.ServerCommands;
using ServerGUI.ServerLogger;

namespace ServerGUI
{
    //MaterialMessageBox.Show("Your cool message here", "The awesome message title");
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        private Task _task;
        private CancellationTokenSource _cancellationTokenSource;

        private Server _server;
        private World World;
        private LoggerManager LoggerManager;
        private ServerCommandHandler ServerCommandHandler;
        private string CommandBox { get; set; } = "";
        private string _filePath { get; set; }
        private DispatcherTimer _timer;

        public MainWindow()
        {
            InitializeComponent();

            World = new World();

            LoggerManager = new LoggerManager();
            _server = new Server(LoggerManager, World);

            PlayersDataGrid.DataContext = World;
            ConsoleDataGrid.DataContext = LoggerManager;
            TxbCommand.DataContext = this;

            var _lock = new object();
            BindingOperations.EnableCollectionSynchronization(LoggerManager.LogMessages, _lock);

            ServerCommandHandler = new ServerCommandHandler(LoggerManager, World);
        }

        private void BtnStart_Click(object sender, RoutedEventArgs e)
        {
            StartTimer();
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
                    LoggerManager.ServerMsg("Could not load map file, Shutting down...");
                    BtnStart.IsEnabled = true;
                    BtnStop.IsEnabled = false;
                    return; //TODO return
                }
                LoggerManager.ServerMsg("Successfully imported map.");
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
                LoggerManager.ServerMsg("Stopping server...");
                _server.UpdateTimer.Change(Timeout.Infinite, Timeout.Infinite);
                _cancellationTokenSource.Cancel();
                BtnStart.IsEnabled = true;
                BtnStop.IsEnabled = false;
                _timer.Stop();
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

        private void TxbCommand_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                BtnCommand_Click(sender, e);
            }
        }

        private void BtnCommand_Click(object sender, RoutedEventArgs e)
        {
            LoggerManager.ServerMsg(CommandBox);
            if (EnterCommand(CommandBox))
            {
                TxbCommand.Text = "";
            }
        }

        private bool EnterCommand(string command)
        {
            string runMessage;
            var successfulRun = ServerCommandHandler.HandleCommandString(command, out runMessage);

            LoggerManager.ServerMsg(runMessage);
            return successfulRun;
        }

        private void StartTimer()
        {
            _timer = new DispatcherTimer();
            _timer.Tick += new EventHandler(TimerEvent);
            _timer.Interval = new TimeSpan(0, 0, 0, 1);
            _timer.Start();
        }
        
        private void TimerEvent(object tempObject, EventArgs e)
        {
            PlayersDataGrid.Items.Refresh();
        }

        public void KickCommandExecute()
        {
            
        }

        public void SetItem(object sender, RoutedEventArgs e)
        {
            var menuItem = (MenuItem)sender;

            var contextMenu = (ContextMenu)menuItem.Parent;

            var item = (DataGrid)contextMenu.PlacementTarget;

            Player player;

            try
            {
                player = (Player)item.SelectedCells[0].Item;
            }
            catch (Exception exception)
            {
                return;
            }

            var command = new KickPlayerCommand();
            command.Run(LoggerManager, null, null, player, World);
        }
    }
}
