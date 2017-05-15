using System;
using System.Collections.Generic;
using System.Linq;
using GeonBit.UI;
using GeonBit.UI.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Lidgren.Network;
using Lidgren;
using Library;
using MonoGame.Extended;

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

        private World _world = new World();

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
            UserInterface.Initialize(Content, "custom");
            UserInterface.UseRenderTarget = true;
            Paragraph.BaseSize = 1.0f;

            Graphics.PreferredBackBufferWidth = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width;
            Graphics.PreferredBackBufferHeight = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height;
            Graphics.IsFullScreen = false;
            Graphics.ApplyChanges();

            _netManager = new NetManager();
            _inputManager = new InputManager(_netManager);

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
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            //if (!IsActive) return;
            
            switch (_state)
            {
                    case GameState.MainMenu:
                        break;
                    case GameState.MainGame:
                        GameLogic(gameTime);
                        break;
            }

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
            }

            _spriteBatch.End();

            UserInterface.DrawMainRenderTarget(_spriteBatch);
            base.Draw(gameTime);
        }

        private void DrawMainMenu(GameTime gameTime)
        {
            _spriteBatch.GraphicsDevice.Clear(Color.White);
        }

        private void GameLogic(GameTime gameTime)
        {
            var elapsed = (float)gameTime.ElapsedGameTime.Milliseconds;
            timer -= elapsed;
            if (timer < 0)
            {
                _netManager.CheckServerMessages();
                timer = TIMER;
            }
            SetCamera(_camera);
            _inputManager.Update();
        }

        private void SetCamera(Camera2D camera)
        {
            var localPlayer = _netManager.World.Players.First(x => x.Name == _netManager.Name);
            //var origin = new Vector2(50, 50);
            _camera.Position = new Vector2(localPlayer.X, localPlayer.Y) - _halfScreen;
        }

        private void DrawGame(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);
            foreach (var circle in _netManager.World.Circles)
            {
                DrawCircle(circle, Color.Red);
            }
            foreach (var player in _netManager.World.Players)
            {
                DrawPlayer(player, Color.Blue);
            }
        }
        private void OldStateChange(GameState state)
        {
            UserInterface.Clear();
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
                    break;
                case GameState.ConnectMenu:
                    var connectMenuPanel = new Panel(new Vector2(500, 500));
                    UserInterface.AddEntity(connectMenuPanel);

                    TextInput nameText = new TextInput(false);
                    nameText.PlaceholderText = "Enter Name";
                    connectMenuPanel.AddChild(nameText);

                    TextInput ipText = new TextInput(false);
                    ipText.PlaceholderText = "Enter host IP";
                    connectMenuPanel.AddChild(ipText);

                    TextInput portText = new TextInput(false);
                    portText.PlaceholderText = "Enter host port";
                    connectMenuPanel.AddChild(portText);

                    //temp

                    nameText.Value = "mecha";
                    ipText.Value = "127.0.0.1";
                    portText.Value = "9911";

                    //temp end

                    var connectButton = new Button("Play");
                    connectButton.ButtonParagraph.Scale = 0.5f;
                    connectButton.OnClick = (Entity btn) =>
                    {
                        if (nameText.Value != "" && ipText.Value != "" && portText.Value != "")
                        {
                            if (_netManager.Initialize(nameText.Value, ipText.Value, int.Parse(portText.Value)))
                            {
                                State = GameState.MainGame;
                            }
                        }
                    };
                    connectMenuPanel.AddChild(connectButton);

                    break;
            }
        }

        void DrawPlayer(Player player, Color color)
        {

            var tempRect = new Rectangle(
                Convert.ToInt16(player.X),
                Convert.ToInt16(player.Y),
                Convert.ToInt16(50 * 2),
                Convert.ToInt16(50 * 2));

            var origin = new Vector2(_playerTexture.Width / 2f, _playerTexture.Height / 2f);

            _spriteBatch.Draw(_playerTexture, destinationRectangle: tempRect, color: Color.Blue, rotation: player.Rotation, origin: origin);
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
    }
}
