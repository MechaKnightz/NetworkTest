using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using EmptyKeys.UserInterface;
using EmptyKeys.UserInterface.Input;
using EmptyKeys.UserInterface.Mvvm;
using GameUILibrary;

namespace GameLibrary.GeneratedUI.MainMenu
{
    public class MainMenuRootViewModel : ViewModelBase
    {
        private Visibility _firstMenuVisible;
        public Visibility FirstMenuVisible
        {
            get { return _firstMenuVisible; }
            set
            {
                _firstMenuVisible = value;
                RaisePropertyChanged("FirstMenuVisible");
            }
        }

        private Visibility _connectMenuVisible;
        public Visibility ConnectMenuVisible
        {
            get { return _connectMenuVisible; }
            set
            {
                _connectMenuVisible = value;
                RaisePropertyChanged("ConnectMenuVisible");
            }
        }

        private Visibility _joinRoomMenuVisible;
        public Visibility JoinRoomMenuVisible
        {
            get { return _joinRoomMenuVisible; }
            set
            {
                _joinRoomMenuVisible = value;
                RaisePropertyChanged("JoinRoomMenuVisible");
            }
        }

        private Visibility _registerMenuVisible;
        public Visibility RegisterMenuVisible
        {
            get { return _registerMenuVisible; }
            set
            {
                _registerMenuVisible = value;
                RaisePropertyChanged("RegisterMenuVisible");
            }
        }

        private Visibility _settingsMenuVisible;
        public Visibility SettingsMenuVisible
        {
            get { return _settingsMenuVisible; }
            set
            {
                _settingsMenuVisible = value;
                RaisePropertyChanged("SettingsMenuVisible");
            }
        }

        public ICommand ExitButtonCommand { get; set; }
        public ICommand PlayButtonCommand { get; set; }
        public ICommand ConnectButtonCommand { get; set; }
        public ICommand BackButtonCommand { get; set; }
        public ICommand BackButton2Command { get; set; }
        public ICommand JoinRoomButtonCommand { get; set; }
        public ICommand RegisterButtonCommand { get; set; }
        public ICommand RegisterCommand { get; set; }
        public ICommand SettingsButtonCommand { get; set; }
        public ICommand VideoSettingsApplyCommand { get; set; }

        public string Username { get; set; }
        public string Password { get; set; }
        public string IP { get; set; }
        public string Port { get; set; }

        public string RoomName { get; set; }

        private ObservableCollection<Resolution> _displayedResolutions;
        public ObservableCollection<Resolution> DisplayedResolutions
        {
            get { return _displayedResolutions; }
            set
            {
                _displayedResolutions = value;
                RaisePropertyChanged("DisplayedResolutions");
            }
        }

        private bool _fullscreen;
        public bool Fullscreen
        {
            get { return _fullscreen; }
            set
            {
                _fullscreen = value;
                RaisePropertyChanged("Fullscreen");
            }
        }

        //public DisplayedResolutionsViewmodel DisplayedResolutionsViewmodel;

        public MainMenuRootViewModel()
        {
            ExitButtonCommand = new RelayCommand(new Action<object>(BtnExit_OnClick));
            PlayButtonCommand = new RelayCommand(new Action<object>(BtnPlay_OnClick));
            ConnectButtonCommand = new RelayCommand(new Action<object>(BtnConnect_OnClick));
            BackButtonCommand = new RelayCommand(new Action<object>(BtnBack_OnClick));
            BackButton2Command = new RelayCommand(new Action<object>(BtnBack2_OnClick));
            JoinRoomButtonCommand = new RelayCommand(new Action<object>(BtnJoinRoom_OnClick));
            RegisterButtonCommand = new RelayCommand(new Action<object>(BtnRegisterMenu_OnClick));
            RegisterCommand = new RelayCommand(new Action<object>(BtnRegister_OnClick));
            SettingsButtonCommand = new RelayCommand(new Action<object>(BtnSettings_OnClick));
            VideoSettingsApplyCommand = new RelayCommand(new Action<object>(BtnVideoSettingsApply_OnClick));

            HideAllUserControls();
            FirstMenuVisible = Visibility.Visible;

            Username = "Username";
            Password = "";
            IP = "terrastructor.ddns.net";
            Port = "9911";
            RoomName = "Room name";
            
            DisplayedResolutions = ToObservableCollection(GetService<IGameService>()?.GetSupportedResolutions());
            //DisplayedResolutionsViewmodel = new DisplayedResolutionsViewmodel();
            //DisplayedResolutionsViewmodel.DisplayedResolutions = ToObservableCollection(GetService<IGameService>()?.GetSupportedResolutions());
        }

        private void BtnBack2_OnClick(object obj)
        {
            HideAllUserControls();

            ConnectMenuVisible = Visibility.Visible;
        }

        private void BtnRegisterMenu_OnClick(object obj)
        {
            HideAllUserControls();
            RegisterMenuVisible = Visibility.Visible;
        }

        private void BtnJoinRoom_OnClick(object obj)
        {
            var gameService = base.GetService<IGameService>(); // or ServiceManager.Instance.GetService
            if (gameService != null)
            {
                if (gameService.JoinRoom(RoomName))
                {
                    HideAllUserControls();
                }
            }
        }

        private void BtnRegister_OnClick(object obj)
        {
            var gameService = base.GetService<IGameService>(); // or ServiceManager.Instance.GetService
            if (gameService != null)
            {
                if (gameService.Register(Username, Password, IP, Port))
                {
                    //todo popup
                }
            }
        }

        private void BtnExit_OnClick(object obj)
        {
            var gameService = base.GetService<IGameService>(); // or ServiceManager.Instance.GetService
            gameService?.Exit();
        }

        private void BtnPlay_OnClick(object obj)
        {
            HideAllUserControls();

            ConnectMenuVisible = Visibility.Visible;
        }

        private void BtnBack_OnClick(object obj)
        {
            HideAllUserControls();

            FirstMenuVisible = Visibility.Visible;
        }

        private void BtnConnect_OnClick(object obj)
        {
            
            var gameService = base.GetService<IGameService>(); // or ServiceManager.Instance.GetService
            if (gameService != null)
            {
                if (gameService.Connect(Username, Password, IP, Port))
                {
                    HideAllUserControls();
                    JoinRoomMenuVisible = Visibility.Visible;
                }
            }
        }

        private void BtnVideoSettingsApply_OnClick(object obj)
        {
            ApplyVideoSettings();
        }

        private void ApplyVideoSettings()
        {
            GetService<IGameService>()?.ToggleFullscreen(Fullscreen);
        }

        private void BtnSettings_OnClick(object obj)
        {
            HideAllUserControls();

            SettingsMenuVisible = Visibility.Visible;
        }

        private void HideAllUserControls()
        {
            FirstMenuVisible = Visibility.Hidden;
            ConnectMenuVisible = Visibility.Hidden;
            JoinRoomMenuVisible = Visibility.Hidden;
            RegisterMenuVisible = Visibility.Hidden;
            SettingsMenuVisible = Visibility.Hidden;
        }

        private static ObservableCollection<T> ToObservableCollection<T>(IEnumerable<T> enumerableList)
        {
            if (enumerableList != null)
            {
                // Create an emtpy observable collection object
                var observableCollection = new ObservableCollection<T>();

                // Loop through all the records and add to observable collection object
                foreach (var item in enumerableList)
                {
                    observableCollection.Add(item);
                }

                // Return the populated observable collection
                return observableCollection;
            }
            return null;
        }
    }
}
