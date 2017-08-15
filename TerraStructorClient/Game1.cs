using System;
using System.Collections.Generic;
using System.Linq;
using EmptyKeys.UserInterface;
using EmptyKeys.UserInterface.Generated;
using EmptyKeys.UserInterface.Mvvm;
using GameLibrary;
using GameUILibrary;
using Library;
using Library.MessageHook;
using Library.PopupHandler;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using TerraStructorClient.GameState;

namespace TerraStructorClient
{
    public class Game1 : Game
    {
        GraphicsDeviceManager Graphics;
        SpriteBatch _spriteBatch;
        public Texture2D _circleTexture, _cursorTexture;
        public NetManager _netManager;
        public InputManager _inputManager;
        public Camera2D _camera;
        private Matrix _viewMatrix;
        public Vector2 _halfScreen;
        public SpriteFont _nameFont;
        private string _tempPassString;
        private Texture2D _tileset;
        private Texture2D _redPixel;
        public Vector2 _chatPadding = new Vector2(5, 5);
        public SpriteFont _chatFont;
        private KeyboardDispatcher _keyboardDispatcher;
        public TextBox _chatTextBox;

        public float timer = 15;
        public float TIMER {get; } = 15;

        private int nativeScreenWidth;
        private int nativeScreenHeight;

        public MainMenuRoot _mainMenuRoot;

        public Game1() : base()
        {
            Graphics = new GraphicsDeviceManager(this);

            Graphics.PreferredBackBufferWidth = 1920;
            Graphics.PreferredBackBufferHeight = 1080;
            Graphics.DeviceCreated += graphics_DeviceCreated;
            Graphics.PreparingDeviceSettings += graphics_PreparingDeviceSettings;

            Content.RootDirectory = "Content";

            ServiceManager.Instance.AddService<IGameService>(new GameService(this));
        }

        void graphics_DeviceCreated(object sender, EventArgs e)
        {
            Engine engine = new MonoGameEngine(GraphicsDevice, nativeScreenWidth, nativeScreenHeight);
        }

        private void graphics_PreparingDeviceSettings(object sender, PreparingDeviceSettingsEventArgs e)
        {
            nativeScreenWidth = Graphics.PreferredBackBufferWidth;
            nativeScreenHeight = Graphics.PreferredBackBufferHeight;

            Graphics.PreferMultiSampling = true;
            Graphics.GraphicsProfile = GraphicsProfile.HiDef;
            Graphics.SynchronizeWithVerticalRetrace = true;
            Graphics.PreferredDepthStencilFormat = DepthFormat.Depth24Stencil8;
            e.GraphicsDeviceInformation.PresentationParameters.MultiSampleCount = 8;
        }

        protected override void Initialize()
        {
            IsMouseVisible = true;

            _keyboardDispatcher = new KeyboardDispatcher(Window);

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

            _circleTexture = Content.Load<Texture2D>("Circle");
            _cursorTexture = Content.Load<Texture2D>("cursor");
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

            GameStateManager.ChangeState(new MainMenuGameState(this));


            //EmptyKeys
            SpriteFont font = Content.Load<SpriteFont>("Segoe_UI_15_Bold");
            FontManager.DefaultFont = Engine.Instance.Renderer.CreateFont(font);
            _mainMenuRoot = new MainMenuRoot();
            _mainMenuRoot.DataContext = new MainMenuRootViewModel();

            FontManager.Instance.LoadFonts(Content);
        }

        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            if (!IsActive) return;
            _inputManager.KeyState = Keyboard.GetState();

            GameStateManager.Update(gameTime);

            _inputManager.OldKeyState = Keyboard.GetState();
            MessageHandler.Update();
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {

            _viewMatrix = _camera.GetViewMatrix();
            _spriteBatch.Begin(transformMatrix: _viewMatrix);

            GameStateManager.Draw(_spriteBatch, gameTime);

            _spriteBatch.End();

            _spriteBatch.Begin();
            MessageHandler.Draw(_spriteBatch);
            _spriteBatch.End();
            base.Draw(gameTime);
        }

        public void InputPredictionUpdate(GameTime gameTime)
        {
            var player = _netManager.CurrentRoom.Players.FirstOrDefault(x => x.Username == _netManager.Username);

            if (player == null) return;

            InputHandler.MoveWithAdjust(new Vector2(player.VelocityX, 0), player, _netManager.CurrentRoom.Map);

            if (player.VelocityX >= player.GravityX) player.VelocityX -= player.GravityX;
            else if (player.VelocityX > 0) player.VelocityX = 0;
            else if (player.VelocityX <= -player.GravityX) player.VelocityX += player.GravityX;
            else if (player.VelocityX < 0) player.VelocityX = 0;
            //player.VelocityX = 0;


            player.VelocityY += player.Gravity;

            if (!InputHandler.MoveWithAdjust(new Vector2(0, player.VelocityY), player, _netManager.CurrentRoom.Map) && player.VelocityY > 0)
            {
                player.VelocityY = 0;
                player.OnGround = true;
                player.IsJumping = false;
            }
            else if (!InputHandler.MoveWithAdjust(new Vector2(0, player.VelocityY), player, _netManager.CurrentRoom.Map) && player.VelocityY < 0)
            {
                player.VelocityY = 0;
            }
            else player.OnGround = false;
        }

        public void SetCamera(Camera2D camera)
        {
            var localPlayer = _netManager.CurrentRoom.Players.FirstOrDefault(x => x.Username == _netManager.Username);

            if (localPlayer == null) return;

            var tempPos = new Vector2(localPlayer.X, localPlayer.Y) - _halfScreen;

            tempPos.X = UsefulMethods.Clamp(tempPos.X,
                0,
                (float)_netManager.CurrentRoom.Map.MapSize * Map.TileSize - _halfScreen.X * 2);

            tempPos.Y = UsefulMethods.Clamp(tempPos.Y,
                0,
                (float)_netManager.CurrentRoom.Map.MapSize * Map.TileSize - _halfScreen.Y * 2);

            _camera.Position = tempPos;
        }

        public void DrawMap(GameTime gameTime)
        {
            _netManager.CurrentRoom.Map.Draw(_spriteBatch, _tileset, _nameFont);
        }

        public void DrawPlayer(Player player, Color color)
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

        public bool Connect(string username, string password, string ip, string port)
        {
            if (username != "" && password != "" && ip != "" && port != "")
            {
                string temp;
                if (_netManager.Initialize(username, password, ip, int.Parse(port),
                    out temp))
                {
                    return true;
                }
                else
                {
                    MessageHandler.CreateMessage(temp);
                    return false;
                }
            }
            return false;
        }

        public void ChangeResolution(int width, int height)
        {
            Graphics.PreferredBackBufferWidth = width;
            Graphics.PreferredBackBufferHeight = height;
        }

        public Resolution GetResolution()
        {
            return new Resolution(Graphics.PreferredBackBufferWidth, Graphics.PreferredBackBufferHeight);

        }

        public List<Resolution> GetSupportedResolutions()
        {
            List<Resolution> resolutions = new List<Resolution>();
            foreach (DisplayMode mode in GraphicsAdapter.DefaultAdapter.SupportedDisplayModes)
            {
                resolutions.Add(new Resolution(mode.Width, mode.Height, mode.AspectRatio));
            }
            return resolutions;
        }

        public bool Register(string username, string password, string ip, string port)
        {
            if (username != "" && password != "" && ip != "" && port != "")
            {
                string temp;
                if (_netManager.Register(username, password, ip, int.Parse(port),
                    out temp))
                {
                    return true;
                }
                else
                {
                    MessageHandler.CreateMessage(temp);
                    return false;
                }
            }
            return false;
        }

        public bool JoinRoom(string roomName)
        {
            string msg;
            if (_inputManager.JoinRoom(roomName, out msg))
            {
                GameStateManager.ChangeState(new MainGameGameState(this));
                return true;
            }
            else
            {
                MessageHandler.CreateMessage(msg);
                return false;
            }
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

        public void DrawMessages(SpriteBatch spriteBatch, List<Library.Messenger.Message> messages, Camera2D camera, IGameState state)
        {
            var length = messages.Count - 10;
            if (messages.Count <= 10)
                length = 0;
            string tempMeasureString = "T";

            if (state is ChatOpenGameState)
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

        public void ChatMessage()
        {
            if (_chatTextBox.Text == "") return;
            _netManager.SendMessage(_chatTextBox.Text);
            GameStateManager.ChangeState(new MainGameGameState(this));
        }
    }
}
