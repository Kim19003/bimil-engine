using Microsoft.Xna.Framework;
using BimilEngine.Source.Engine.Objects;
using BimilEngine.Source.Engine.Objects.Bases;
using BimilEngine.Source.Engine.Models;
using BimilEngine.Source.Engine.Managers;
using Microsoft.Xna.Framework.Input;
using System.Linq;
using BimilEngine.Source.Engine.Other;
using System.Runtime.Serialization;

namespace BimilEngine.Source.GameLogic.Gadgets
{
    public class GameController : Gadget2D
    {
        public GameController(Vector2 position, Vector2 scale, int cameraLevel = 0, string name = "", string tag = "", Scene2D associatedScene = null)
            : base(position, scale, cameraLevel, name, tag, associatedScene)
        {
        }

        public override void Start()
        {
            // ---------
            base.Start();
        }

        bool isDebug = false;
        Log log1 = null, log2 = null;
        public override void Update(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            KeyboardState keyboardState = Keyboard.GetState();
            Camera2D activeCamera = Environment2D.ActiveScene.ActiveCameras.FirstOrDefault();

            if (isDebug)
            {
                ShadowSettings shadowSettings = new(new Vector2(1, 1), new(0, 0, 0, 0.75f));

                if (log2 == null)
                    log2 = LogManager.DoScreenLog($"FPS: {Environment2D.ScreenHandler.FramesPerSecondInt}", LogLevel.Information, 0, shadowSettings);
                else
                    log2.Message = $"FPS: {Environment2D.ScreenHandler.FramesPerSecondInt}";

                if (log1 == null)
                    log1 = LogManager.DoScreenLog($"Camera depth: {activeCamera.Depth}", LogLevel.Error, 0, shadowSettings);
                else
                    log1.Message = $"Camera depth: {activeCamera.Depth}";

                Environment2D.GridSettings.CellSize = new(32, 32);
                // Environment2D.GridSettings.SizeMultiplier = 10;
                Environment2D.GridSettings.Enabled = true;
            }
            else
            {
                LogManager.ClearShownScreenLogs();
                log1 = null;
                log2 = null;
                Environment2D.GridSettings.Enabled = false;
            }

            if (keyboardState.IsKeyPressed(Keys.R))
                Environment2D.LoadScene("Main");
            else if (keyboardState.IsKeyPressed(Keys.T))
                Environment2D.LoadScene("Second");
            else if (keyboardState.IsKeyPressed(Keys.F))
            {
                Environment2D.ScreenHandler.Width = Environment2D.ScreenHandler.Width == Environment2D.DEFAULT_SCREEN_WIDTH ? 1920 : Environment2D.DEFAULT_SCREEN_WIDTH;
                Environment2D.ScreenHandler.Height = Environment2D.ScreenHandler.Height == Environment2D.DEFAULT_SCREEN_HEIGHT ? 1080 : Environment2D.DEFAULT_SCREEN_HEIGHT;
                activeCamera.Scale = new(Environment2D.ScreenHandler.Width, Environment2D.ScreenHandler.Height);
                Environment2D.ScreenHandler.IsFullScreen = !Environment2D.ScreenHandler.IsFullScreen;
                Environment2D.ScreenHandler.ApplyChanges();
            }
            else if (keyboardState.IsKeyPressed(Keys.PageDown))
            {
                activeCamera.Depth -= 1f;
            }
            else if (keyboardState.IsKeyPressed(Keys.PageUp))
            {
                activeCamera.Depth += 1f;
            }
            else if (keyboardState.IsKeyPressed(Keys.P))
            {
                if (Environment2D.ScreenHandler.MaxFramesPerSecond == 60)
                    Environment2D.ScreenHandler.MaxFramesPerSecond = Environment2D.DEFAULT_MAX_FPS;
                else
                    Environment2D.ScreenHandler.MaxFramesPerSecond = 60;
            }
            else if (keyboardState.IsKeyPressed(Keys.O))
            {
                if (Environment2D.ScreenHandler.MaxFramesPerSecond == 60)
                    Environment2D.ScreenHandler.MaxFramesPerSecond = 10;
                else
                    Environment2D.ScreenHandler.MaxFramesPerSecond = 60;
            }
            else if (keyboardState.IsKeyPressed(Keys.G))
            {
                isDebug = !isDebug;
            }
            
            if (keyboardState.IsKeyDown(Keys.A))
            {
                activeCamera.MatrixPosition -= new Vector2(0.1f, 0) * deltaTime * 1000;
            }
            if (keyboardState.IsKeyDown(Keys.D))
            {
                activeCamera.MatrixPosition += new Vector2(0.1f, 0) * deltaTime * 1000;
            }
            if (keyboardState.IsKeyDown(Keys.W))
            {
                activeCamera.MatrixPosition -= new Vector2(0, 0.1f) * deltaTime * 1000;
            }
            if (keyboardState.IsKeyDown(Keys.S))
            {
                activeCamera.MatrixPosition += new Vector2(0, 0.1f) * deltaTime * 1000;
            }

            if (keyboardState.IsKeyPressed(Keys.K))
            {
                Root.TimeScale = 0.1f;
            }
            else if (keyboardState.IsKeyPressed(Keys.L))
            {
                Root.TimeScale = 1;
            }

            // ---------
            base.Update(gameTime);
        }

        public override void FixedUpdate(GameTime gameTime, GameTime fixedGameTime)
        {

            // ---------
            base.FixedUpdate(gameTime, fixedGameTime);
        }
    }
}