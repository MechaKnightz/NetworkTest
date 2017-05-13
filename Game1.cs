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
        GraphicsDeviceManager graphics;
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
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            _camera = new Camera2D(GraphicsDevice);
            _state = State.Main;
            _viewMatrix = _camera.GetViewMatrix();
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
                        Main();
                        break;
                    case State.CreatingRect:
                        CreatingCircle();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

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

        private void Main()
        {
            if (keyState.IsKeyDown(Keys.S) && keyState.IsKeyDown(Keys.LeftControl))
            {
                SaveMap();
                Exit();
            }
            if (keyState.IsKeyDown(Keys.L) && keyState.IsKeyDown(Keys.LeftControl))
            {
                LoadMap();
            }
            if (!MouseInput.IsLeftKeyClicked()) return;
            selectedCircle = new Circle(0, _camera.ScreenToWorld(MouseInput.mouseState.Position.ToVector2()));
            _state = State.CreatingRect;
        }

        private void SaveMap()
        {

            string saveString = JsonConvert.SerializeObject(_circles, Formatting.Indented);

            var destPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "map1" + ".json");

            File.WriteAllText(destPath, saveString);
        }

        private void LoadMap()
        {
            string destPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "map1" + ".json");

            var saveString = File.ReadAllText(destPath);

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
