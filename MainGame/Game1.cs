using System;
using System.Collections.Generic;
using System.Linq;
using GeonBit.UI;
using GeonBit.UI.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Library;
using Library.MessageHook;
using Library.Messenger;
using Library.PopupHandler;
using MonoGame.Extended;
using Button = GeonBit.UI.Entities.Button;
using Keys = Microsoft.Xna.Framework.Input.Keys;
using Panel = GeonBit.UI.Entities.Panel;

namespace MainGame
{
    public class Game1 : Game
    {
        GraphicsDeviceManager Graphics;
        SpriteBatch _spriteBatch;
        private Texture2D _circleTexture, _playerTexture;
        private NetManager _netManager;
        private InputManager _inputManager;
        private Camera2D _camera;
        private Matrix _viewMatrix;
        private Vector2 _halfScreen;
        private InputHelper _inputHelper;
        private SpriteFont _nameFont;
        private string _tempPassString;
        private Texture2D _tileset;
        private Texture2D _redPixel;
        private Vector2 _chatPadding = new Vector2(5, 5);
        private SpriteFont _chatFont;
        private KeyboardDispatcher _keyboardDispatcher;
        private TextBox _chatTextBox;

        float timer = 15;
        const float TIMER = 15;

        private GameState _state;
        private GameState State
        {
            set
            {
                OldStateChange(_state);
                _state = value;
                ChangeState(_state);
            }
            get { return _state; }
        }

        public Game1()
        {
            Graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }
        protected override void Initialize()
        {
            _keyboardDispatcher = new KeyboardDispatcher(Window);

            UserInterface.Initialize(Content, "custom");
            UserInterface.UseRenderTarget = true;
            Paragraph.BaseSize = 1.0f;

            var form = (System.Windows.Forms.Form)System.Windows.Forms.Control.FromHandle(Window.Handle);
            form.Location = new System.Drawing.Point(0, 0);

            Graphics.PreferredBackBufferWidth = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width;
            Graphics.PreferredBackBufferHeight = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height;
            Graphics.IsFullScreen = false;
            Graphics.ApplyChanges();

            _netManager = new NetManager();
            _inputManager = new InputManager(_netManager);

            MessageHandler.Initialize(
                Content.Load<Texture2D>("BoxTexture"),
                Content.Load<SpriteFont>("BoxFont"),
                Color.Black);

            _camera = new Camera2D(GraphicsDevice);
            _viewMatrix = _camera.GetViewMatrix();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _halfScreen = new Vector2(Graphics.PreferredBackBufferWidth / 2f, Graphics.PreferredBackBufferHeight / 2f);


            State = GameState.MainMenu;
            _circleTexture = Content.Load<Texture2D>("Circle");
            _playerTexture = Content.Load<Texture2D>("PlayerCircle");
            _nameFont = Content.Load<SpriteFont>("BoxFont");
            _chatFont = Content.Load<SpriteFont>("BoxFont");
            _tileset = Content.Load<Texture2D>("Tileset");
            _redPixel = Content.Load<Texture2D>("RedPixel");
            _chatTextBox = new TextBox(Content.Load<Texture2D>("TextBox"), Content.Load<Texture2D>("Caret"), _chatFont)
            {
                X = 0,
                Y = 0,
                Width = 300
            };
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            if (!IsActive) return;
            _inputManager.KeyState = Keyboard.GetState();

            switch (_state)
            {

                case GameState.MainMenu:
                    break;
                case GameState.MainGame:
                    GameLogic(gameTime);
                    break;
                case GameState.ChatOpen:
                    ChatLogic(gameTime);
                    break;
            }
            _inputManager.OldKeyState = Keyboard.GetState();
            MessageHandler.Update();
            UserInterface.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            UserInterface.Draw(_spriteBatch);

            _viewMatrix = _camera.GetViewMatrix();
            _spriteBatch.Begin(transformMatrix: _viewMatrix);

            switch (_state)
            {
                case GameState.MainMenu:
                    DrawMainMenu(gameTime);
                    break;
                case GameState.MainGame:
                    DrawGame(gameTime);
                    break;
                case GameState.ChatOpen:
                    DrawGame(gameTime);
                    DrawChatOpen(gameTime);
                    break;
            }
            _spriteBatch.End();

            UserInterface.DrawMainRenderTarget(_spriteBatch);
            _spriteBatch.Begin();
            MessageHandler.Draw(_spriteBatch);
            _spriteBatch.End();
            base.Draw(gameTime);
        }

        private void DrawMainMenu(GameTime gameTime)
        {
            _spriteBatch.GraphicsDevice.Clear(Color.Black);
        }

        private void GameLogic(GameTime gameTime)
        {
            var elapsed = (float)gameTime.ElapsedGameTime.Milliseconds;
            timer -= elapsed;
            if (timer < 0)
            {
                _netManager.CheckServerMessages(gameTime);
                timer = TIMER;
            }
            if (_inputManager.IsKeyClicked(Keys.Enter))
                State = GameState.ChatOpen;
            SetCamera(_camera);
            _inputManager.Update(_camera);
            
        }

        private void SetCamera(Camera2D camera)
        {
            var localPlayer = _netManager.CurrentRoom.Players.FirstOrDefault(x => x.Username == _netManager.Username);

            if (localPlayer == null) return;

            var tempPos = new Vector2(localPlayer.X, localPlayer.Y) - _halfScreen;

            tempPos.X = Clamp(tempPos.X,
                0,
                (float)_netManager.CurrentRoom.Map.MapSize * Map.TileSize - _halfScreen.X * 2);

            tempPos.Y = Clamp(tempPos.Y,
                0,
                (float) _netManager.CurrentRoom.Map.MapSize * Map.TileSize - _halfScreen.Y * 2);

            _camera.Position = tempPos;
        }

        private void DrawGame(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            DrawMap(gameTime);
            Player localPlayer = new Player();
            for (int i = 0; i < _netManager.CurrentRoom.Players.Count; i++)
            {
                DrawPlayer(_netManager.CurrentRoom.Players[i], Color.Red);
                if (_netManager.CurrentRoom.Players[i].Username == _netManager.Username)
                    localPlayer = _netManager.CurrentRoom.Players[i];
            }

            _spriteBatch.DrawString(_nameFont, new Vector2(localPlayer.X, localPlayer.Y).ToString(), _camera.ScreenToWorld(0, 0), Color.White);
            _spriteBatch.DrawString(_nameFont, gameTime.TotalGameTime.ToString(), _camera.ScreenToWorld(0, 50), Color.White);
            _spriteBatch.DrawString(_nameFont, _netManager.CurrentRoom.Name, _camera.ScreenToWorld(0, 100), Color.White);

            DrawMessages(_spriteBatch, _netManager.ChatMessages, _camera);
        }

        private void DrawMap(GameTime gameTime)
        {
            _netManager.CurrentRoom.Map.Draw(_spriteBatch, _tileset, _nameFont);
        }

        private void OldStateChange(GameState state)
        {
            UserInterface.Clear();
        }

        private void ChatLogic(GameTime gameTime)
        {
            _chatTextBox.Update(gameTime);
            var elapsed = (float)gameTime.ElapsedGameTime.Milliseconds;
            timer -= elapsed;
            if (timer < 0)
            {
                _netManager.CheckServerMessages(gameTime);
                timer = TIMER;
            }
            SetCamera(_camera);
            if (_inputManager.IsKeyClicked(Keys.Escape))
                State = GameState.MainGame;
            if (_inputManager.IsKeyClicked(Keys.Enter))
            {
                ChatMessage();
            }
            _chatTextBox.X = Convert.ToInt16(_camera.ScreenToWorld(_chatPadding.X, 0).X);
            var value = _halfScreen.Y * 2 - _chatPadding.Y - _chatFont.MeasureString("T").Y;
            _chatTextBox.Y = Convert.ToInt16(_camera.ScreenToWorld(0, value).Y);
        }

        private void DrawChatOpen(GameTime gameTime)
        {
            _chatTextBox.Draw(_spriteBatch, gameTime);
        }

        void DrawPlayer(Player player, Color color)
        {

            var tempRect = new Rectangle(
                Convert.ToInt16(player.X),
                Convert.ToInt16(player.Y),
                Convert.ToInt16(Player.Width),
                Convert.ToInt16(Player.Height));

            _spriteBatch.Draw(_redPixel, tempRect, color);

            _spriteBatch.DrawString(_nameFont,
                player.Username,
                new Vector2(player.X - _nameFont.MeasureString(player.Username).X / 2f,
                player.Y - _nameFont.MeasureString(player.Username).Y / 2f),
                Color.White);

            _spriteBatch.DrawString(_nameFont,
                player.Health.ToString(),
                new Vector2(player.X - _nameFont.MeasureString(player.Health.ToString()).X / 2f,
                    player.Y - _nameFont.MeasureString(player.Health.ToString()).Y / 2f - 50),
                Color.White);
        }

        public void DrawCircle(Circle circle, Color color)
        {
            if (circle.Radius < 1) return;

            var diameter = circle.Radius * 2;

            var tempRect = new Rectangle(
                Convert.ToInt16(circle.X - circle.Radius),
                Convert.ToInt16(circle.Y - circle.Radius),
                Convert.ToInt16(diameter),
                Convert.ToInt16(diameter));

            _spriteBatch.Draw(_circleTexture, tempRect, color);
        }

        public void DrawRegisterMenu()
        {

            var connectMenuPanel = new Panel(new Vector2(500, 500));
            UserInterface.AddEntity(connectMenuPanel);

            var connectButton = new Button("Register");
            connectButton.ButtonParagraph.Scale = 0.5f;
            
            connectMenuPanel.AddChild(connectButton);

            TextInput nameText = new TextInput(false);
            nameText.PlaceholderText = "Enter Username";
            connectMenuPanel.AddChild(nameText);

            TextInput passText = new TextInput(false);
            passText.PlaceholderText = "Enter password";
            connectMenuPanel.AddChild(passText);
            passText.OnValueChange = entity =>
            {
                if (_tempPassString == null) _tempPassString = "";
                if (_tempPassString.Length > passText.Value.Length)
                {
                    _tempPassString = _tempPassString.Substring(0, passText.Value.Length);
                }
                var tempString = passText.Value;
                passText.Value = new string('*', passText.Value.Length);
                tempString = tempString.Replace("*", "");
                _tempPassString += tempString;
            };

            TextInput ipText = new TextInput(false);
            ipText.PlaceholderText = "Enter host IP";
            connectMenuPanel.AddChild(ipText);

            TextInput portText = new TextInput(false);
            portText.PlaceholderText = "Enter host port";
            connectMenuPanel.AddChild(portText);

            var backButton = new Button("Back");
            backButton.ButtonParagraph.Scale = 0.5f;
            backButton.OnClick = (Entity btn) =>
            {
                State = GameState.MainMenu;
            };
            connectMenuPanel.AddChild(backButton);

            connectButton.OnClick = (Entity btn) =>
            {
                if (nameText.Value != "" && ipText.Value != "" && portText.Value != "" && passText.Value != "")
                {
                    if (_tempPassString == null) _tempPassString = passText.Value;
                    string temp;
                    if (_netManager.Register(nameText.Value, _tempPassString, ipText.Value, int.Parse(portText.Value),
                        out temp))
                        MessageHandler.CreateMessage(temp);
                }
            };

            //temp
            ipText.Value = "127.0.0.1";
            portText.Value = "9911";
            //temp end
        }

        private void ChangeState(GameState state)
        {
            switch (state)
            {
                case GameState.MainMenu:
                    var mainMenuPanel = new Panel(new Vector2(300, 500));
                    UserInterface.AddEntity(mainMenuPanel);

                    var mainMenuPlayButton = new Button("Play");
                    mainMenuPlayButton.ButtonParagraph.Scale = 0.5f;
                    mainMenuPlayButton.OnClick = (Entity btn) =>
                    {
                        State = GameState.ConnectMenu;
                    };
                    mainMenuPanel.AddChild(mainMenuPlayButton);

                    var mainMenuRegisterButton = new Button("Register");
                    mainMenuRegisterButton.ButtonParagraph.Scale = 0.5f;
                    mainMenuRegisterButton.OnClick = (Entity btn) =>
                    {
                        State = GameState.RegisterMenu;
                    };
                    mainMenuPanel.AddChild(mainMenuRegisterButton);
                    break;
                case GameState.ConnectMenu:
                    var connectMenuPanel = new Panel(new Vector2(700, 500));
                    UserInterface.AddEntity(connectMenuPanel);
                    var connectButton = new Button("Connect");
                    connectButton.ButtonParagraph.Scale = 0.5f;

                    connectMenuPanel.AddChild(connectButton);

                    TextInput nameText = new TextInput(false);
                    nameText.PlaceholderText = "Enter Username";
                    connectMenuPanel.AddChild(nameText);

                    TextInput passText = new TextInput(false);
                    passText.PlaceholderText = "Enter password";
                    connectMenuPanel.AddChild(passText);
                    passText.OnValueChange = entity =>
                    {
                        if (_tempPassString == null) _tempPassString = "";
                        if (_tempPassString.Length > passText.Value.Length)
                        {
                            _tempPassString = _tempPassString.Substring(0, passText.Value.Length);
                        }
                        var tempString = passText.Value;
                        passText.Value = new string('*', passText.Value.Length);
                        tempString = tempString.Replace("*", "");
                        _tempPassString += tempString;
                    };

                    TextInput ipText = new TextInput(false);
                    ipText.PlaceholderText = "Enter host IP";
                    connectMenuPanel.AddChild(ipText);

                    TextInput portText = new TextInput(false);
                    portText.PlaceholderText = "Enter host port";
                    connectMenuPanel.AddChild(portText);

                    //temp

                    nameText.Value = "mecha";
                    passText.Value = "test";
                    ipText.Value = "127.0.0.1";
                    portText.Value = "9911";

                    //temp end

                    var backButton = new Button("Back");
                    backButton.ButtonParagraph.Scale = 0.5f;
                    backButton.OnClick = (Entity btn) =>
                    {
                        State = GameState.MainMenu;
                    };
                    connectMenuPanel.AddChild(backButton);

                    connectButton.OnClick = (Entity btn) =>
                    {
                        if (nameText.Value != "" && ipText.Value != "" && portText.Value != "")
                        {
                            if (_tempPassString == null) _tempPassString = passText.Value;
                            string temp;
                            if (_netManager.Initialize(nameText.Value, _tempPassString, ipText.Value, int.Parse(portText.Value), out temp))
                            {
                                State = GameState.RoomConnectMenu;
                            }
                            else
                            {
                                MessageHandler.CreateMessage(temp);
                            }
                        }
                    };

                    break;
                case GameState.RoomConnectMenu:
                    var connectRoomMenuPanel = new Panel(new Vector2(500, 500));
                    UserInterface.AddEntity(connectRoomMenuPanel);

                    TextInput roomNameText = new TextInput(false);
                    roomNameText.PlaceholderText = "Enter room name";

                    connectRoomMenuPanel.AddChild(roomNameText);

                    var connectRoomButton = new Button("Join room");
                    connectRoomButton.ButtonParagraph.Scale = 0.5f;

                    connectRoomButton.OnClick = entity =>
                    {
                        string msg;
                        if (_inputManager.JoinRoom(roomNameText.Value, out msg))
                        {
                            State = GameState.MainGame;
                        }
                        else
                        {
                            MessageHandler.CreateMessage(msg);
                        }
                    };

                    connectRoomMenuPanel.AddChild(connectRoomButton);
                    break;
                case GameState.RegisterMenu:
                    DrawRegisterMenu();
                    break;
                case GameState.ChatOpen:
                    _keyboardDispatcher.Subscriber = _chatTextBox;
                    _chatTextBox.Text = "";
                    //_chatTextBox.OnEnterPressed += sender =>
                    //{
                    //    if (sender.Text == "") return;
                    //    _netManager.ChatMessages.Add(new Library.Messenger.Message(sender.Text, "tester"));
                    //    State = GameState.MainGame;
                    //};
                    break;
            }
        }

        public static float Clamp(float value, float min, float max)
        {
            return (value < min) ? min : (value > max) ? max : value;
        }

        private void DrawMessages(SpriteBatch spriteBatch, List<Library.Messenger.Message> messages, Camera2D camera)
        {
            var length = messages.Count - 10;
            if (messages.Count <= 10)
                length = 0;
            string tempMeasureString = "T";

            if (State == GameState.ChatOpen)
            {
                int j = 1;
                for (int i = messages.Count - 1; i >= length; i--)
                {
                    var drawPos =
                        camera.ScreenToWorld(new Vector2(_chatPadding.X, _halfScreen.Y * 2) -
                                             new Vector2(0,
                                                 (j + 1) *
                                                 (_chatPadding.Y + _chatFont.MeasureString(tempMeasureString).Y)));
                    var message = messages[i].GetMessage();

                    spriteBatch.DrawString(_chatFont, message, drawPos + Vector2.One, Color.Black);

                    spriteBatch.DrawString(
                        _chatFont,
                        message,
                        drawPos,
                        Color.White);

                    j++;
                }
            }
            else
            {
                int j = 0;
                for (int i = messages.Count - 1; i >= length; i--)
                {
                    if (messages[i].Timestamp + TimeSpan.FromSeconds(5) < DateTime.Now) break;
                    var drawPos =
                        camera.ScreenToWorld(new Vector2(_chatPadding.X, _halfScreen.Y * 2) -
                                             new Vector2(0,
                                                 (j + 1) *
                                                 (_chatPadding.Y + _chatFont.MeasureString(tempMeasureString).Y)));
                    var message = messages[i].GetMessage();

                    spriteBatch.DrawString(_chatFont, message, drawPos + Vector2.One, Color.Black);

                    spriteBatch.DrawString(
                        _chatFont,
                        message,
                        drawPos,
                        Color.White);

                    j++;
                }
            }
        }

        private void ChatMessage()
        {
            if (_chatTextBox.Text == "") return;
            _netManager.SendMessage(_chatTextBox.Text);
            State = GameState.MainGame;
        }
    }
}
