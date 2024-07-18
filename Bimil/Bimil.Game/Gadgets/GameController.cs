using Microsoft.Xna.Framework;
using Bimil.Engine.Objects;
using Bimil.Engine.Objects.Bases;
using Bimil.Engine.Models;
using Bimil.Engine.Managers;
using Microsoft.Xna.Framework.Input;
using System.Linq;
using Bimil.Engine.Other;
using Bimil.Engine;
using Bimil.Game.Scenes;

namespace Bimil.Game.Gadgets
{
    public class GameController : Gadget2D
    {
        public GameController(Scene2D associatedScene = null)
            : base(associatedScene)
        {
        }

        public override void Start()
        {
            if (!Root.Core.AudioHandler.Songs.ContainsKey("Main Theme"))
                Root.Core.AudioHandler.Songs.Add("Main Theme", Root.SongBatch["Main Theme"]);
            // Root.EngineCore.AudioHandler.PlaySong("Main Theme");

            //Globals.Root.EngineCore.LoadScene("asd");

            // ---------
            base.Start();
        }

        bool isDebug = false;
        Log log1 = null, log2 = null;
        public override void Update(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            KeyboardState keyboardState = Keyboard.GetState();
            Camera2D activeCamera = Root.Core.ActiveScene.ActiveCameras.FirstOrDefault();

            if (isDebug)
            {
                ShadowSettings shadowSettings = new(new Vector2(1, 1), new(0, 0, 0, 0.75f));

                if (log2 == null)
                    log2 = LogManager.DoScreenLog($"FPS: {Root.Core.ScreenHandler.FramesPerSecondInt}", LogLevel.Information, 0, shadowSettings);
                else
                    log2.Message = $"FPS: {Root.Core.ScreenHandler.FramesPerSecondInt}";

                if (log1 == null)
                    log1 = LogManager.DoScreenLog($"Camera depth: {activeCamera.Depth}", LogLevel.Error, 0, shadowSettings);
                else
                    log1.Message = $"Camera depth: {activeCamera.Depth}";

                Root.Core.GridSettings.CellSize = new(32, 32);
                // Root.EngineCore.GridSettings.SizeMultiplier = 10;
                Root.Core.GridSettings.Enabled = true;
            }
            else
            {
                LogManager.ClearShownScreenLogs();
                log1 = null;
                log2 = null;
                Root.Core.GridSettings.Enabled = false;
            }

            if (keyboardState.IsKeyPressed(Keys.R))
                Root.Core.SceneHandler.LoadScene(nameof(Main));
            else if (keyboardState.IsKeyPressed(Keys.T))
                Root.Core.SceneHandler.LoadScene(nameof(Secondary));
            else if (keyboardState.IsKeyPressed(Keys.F))
            {
                Root.Core.ScreenHandler.Width = Root.Core.ScreenHandler.Width == Root.Settings.DefaultScreenWidth
                    ? 1920
                    : Root.Settings.DefaultScreenWidth;
                Root.Core.ScreenHandler.Height = Root.Core.ScreenHandler.Height == Root.Settings.DefaultScreenHeight
                    ? 1080
                    : Root.Settings.DefaultScreenHeight;
               activeCamera.Scale = new(Root.Core.ScreenHandler.Width, Root.Core.ScreenHandler.Height);
                Root.Core.ScreenHandler.IsFullScreen = !Root.Core.ScreenHandler.IsFullScreen;
                Root.Core.ScreenHandler.ApplyChanges();
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
               if (Root.Core.ScreenHandler.MaxFramesPerSecond == 60)
                    Root.Core.ScreenHandler.MaxFramesPerSecond = Root.Settings.DefaultMaxFPS;
               else
                    Root.Core.ScreenHandler.MaxFramesPerSecond = 60;
            }
            else if (keyboardState.IsKeyPressed(Keys.O))
            {
               if (Root.Core.ScreenHandler.MaxFramesPerSecond == 60)
                    Root.Core.ScreenHandler.MaxFramesPerSecond = 10;
               else
                    Root.Core.ScreenHandler.MaxFramesPerSecond = 60;
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
                Root.Core.TimeScale = 0.1f;
            }
            else if (keyboardState.IsKeyPressed(Keys.L))
            {
                Root.Core.TimeScale = 1;
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