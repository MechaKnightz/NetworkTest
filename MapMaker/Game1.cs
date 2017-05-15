using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Library;

namespace MapMaker
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager Graphics;
        SpriteBatch spriteBatch;
        private Texture2D _circleTexture;
        private State _state;
        private List<Circle> _circles = new List<Circle>();
        private KeyboardState keyState, oldKeyState;
        private Circle selectedCircle;
        private Camera2D _camera;
        private Matrix _viewMatrix;

        public Game1()
        {
            Graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            _camera = new Camera2D(GraphicsDevice);
            _state = State.Main;
            _viewMatrix = _camera.GetViewMatrix();
            MessageHandler.Initialize(
                Content.Load<Texture2D>("BoxTexture"),
                Content.Load<SpriteFont>("BoxFont"),
                Color.Black);

            Graphics.PreferredBackBufferWidth = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width;
            Graphics.PreferredBackBufferHeight = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height;
            Graphics.IsFullScreen = false;
            Graphics.ApplyChanges();

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            IsMouseVisible = true;
            _circleTexture = Content.Load<Texture2D>("Circle");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (!IsActive)
                return;

            MouseInput.mouseState = Mouse.GetState();
            keyState = Keyboard.GetState();
            UpdateCamera();

            switch(_state)
                {
                    case State.Main:
                        Main(gameTime);
                        break;
                    case State.CreatingRect:
                        CreatingCircle();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

            MessageHandler.Update(gameTime);
            MouseInput.oldMouseState = Mouse.GetState();
            oldKeyState = Keyboard.GetState();
            base.Update(gameTime);
        }

        private void UpdateCamera()
        {
            if (keyState.IsKeyDown(Keys.D)) _camera.Position += new Vector2(10, 0);
            if (keyState.IsKeyDown(Keys.W)) _camera.Position += new Vector2(0, -10);
            if (keyState.IsKeyDown(Keys.A)) _camera.Position += new Vector2(-10, 0);
            if (keyState.IsKeyDown(Keys.S)) _camera.Position += new Vector2(0, 10);
            if (keyState.IsKeyDown(Keys.E)) _camera.Zoom += 0.015f;
            else if (keyState.IsKeyDown(Keys.Q)) _camera.Zoom -= 0.015f;

        }

        private void CreatingCircle()
        {
            selectedCircle.Radius = Vector2.Distance(new Vector2(selectedCircle.X, selectedCircle.Y), _camera.ScreenToWorld(MouseInput.mouseState.Position.ToVector2()));
            if (keyState.IsKeyDown(Keys.Escape))
            {
                _state = State.Main;
                selectedCircle.Radius = 0;
            }
            if (MouseInput.IsLeftKeyClicked())
            {
                _circles.Add(selectedCircle);
                _state = State.Main;
            }
        }

        private void Main(GameTime gameTime)
        {
            if (keyState.IsKeyDown(Keys.S) && keyState.IsKeyDown(Keys.LeftControl))
            {
                SaveMap();
                Exit();
            }
            if (keyState.IsKeyDown(Keys.L) && keyState.IsKeyDown(Keys.LeftControl))
            {
                LoadMap(gameTime);
            }
            if (!MouseInput.IsLeftKeyClicked()) return;
            selectedCircle = new Circle(0, _camera.ScreenToWorld(MouseInput.mouseState.Position.ToVector2()));
            _state = State.CreatingRect;
        }

        private void SaveMap()
        {

            string saveString = JsonConvert.SerializeObject(_circles, Formatting.Indented);

            string destPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\MapMaker\";

            var savePath = Path.Combine(destPath, "map1" + ".json");

            Directory.CreateDirectory(destPath);
            File.WriteAllText(savePath, saveString);

        }

        private void LoadMap(GameTime gameTime)
        {
            string destPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\MapMaker\";

            var savePath = Path.Combine(destPath, "map1" + ".json");


            string saveString = "";

            Directory.CreateDirectory(destPath);
            try
            {
                saveString = File.ReadAllText(savePath);
            }
            catch (Exception e)
            {
                MessageHandler.CreateMessage(e.Message, gameTime);
                return;
            }
            _circles = JsonConvert.DeserializeObject<List<Circle>>(saveString);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            _viewMatrix = _camera.GetViewMatrix();
            spriteBatch.Begin(transformMatrix: _viewMatrix);

            DrawCircle(selectedCircle, Color.Red);
            foreach (var circle in _circles)
            {
                DrawCircle(circle, Color.Blue);
            }

            MessageHandler.Draw(spriteBatch);

            spriteBatch.End();

            base.Draw(gameTime);
        }

        public void DrawCircle(Circle circle, Color color)
        {
            if(circle.Radius < 1) return;

            var diameter = circle.Radius * 2;

            var tempRect = new Rectangle(
                Convert.ToInt16(circle.X - circle.Radius),
                Convert.ToInt16(circle.Y - circle.Radius),
                Convert.ToInt16(diameter),
                Convert.ToInt16(diameter));

            spriteBatch.Draw(_circleTexture, tempRect, color);
        }
    }
}
