using System.Linq;
using Microsoft.Xna.Framework;
using Bimil.Engine.Handlers;
using Bimil.Engine.Functions;
using Genbox.VelcroPhysics.Dynamics;
using Bimil.Engine.Models;
using Microsoft.Xna.Framework.Graphics;
using Bimil.Engine.Objects;
using Bimil.Engine.Managers;
using System;
using Bimil.Engine.Objects.Bases;

namespace Bimil.Engine
{
    public class Core : Game
    {
        /// <summary>
        /// Handler, that handles everything screen related.
        /// </summary>
        public ScreenHandler ScreenHandler { get; } = new();
        /// <summary>
        /// Handler, that handles all the audio.
        /// </summary>
        public AudioHandler AudioHandler { get; } = new();
        /// <summary>
        /// Grid settings used in the grid drawing. Set Enabled to true to enable the grid.
        /// </summary>
        public GridSettings GridSettings { get; } = new();
        /// <summary>
        /// Handler, that handles all the scenes.
        /// </summary>
        public SceneHandler SceneHandler { get; } = new();

        /// <summary>
        /// The physics world (this handles all the physics).
        /// </summary>
        public World PhysicsWorld { get; protected set; }

        /// <summary>
        /// The active scene.
        /// </summary>
        public Scene2D ActiveScene => SceneHandler.ActiveScene;

        /// <summary>
        /// The fixed update time step of the game.
        /// </summary>
        public static float FixedUpdateTimeStep => 1 / Root.Settings.PhysicsHertz;
        /// <summary>
        /// The update time step of the game.
        /// </summary>
        public float UpdateTimeStep => ScreenHandler.MaxFramesPerSecond > 0f
            ? 1f / ScreenHandler.MaxFramesPerSecond
            : 0.00001f;

        /// <summary>
        /// The time scale of the game. This is used for the time manipulation.
        /// </summary>
        public float TimeScale { get; set; } = 1f;

        /// <summary>
        /// The calculated interpolation alpha. The calculation is done in the Update method.
        /// </summary>
        public float InterpolationAlpha { get; private set; } = 0f;

        /// <summary>
        /// Is it the first update call?
        /// </summary>
        public bool IsFirstUpdateCall { get; protected set; } = true;

        public Core()
        {
            Root.Graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            IsFixedTimeStep = true; // Use fixed time step. This is used for the FPS limiting. You shouldn't change this.
            TargetElapsedTime = TimeSpan.FromSeconds(UpdateTimeStep); // Set the fixed time step with UpdateTimeStep (basically the FPS).

            // Tip: Use ScreenHandler.UseVsync to control the VSync
            // Tip: Use ScreenHandler.MaxFramesPerSecond to control the FPS limit

            Root.Core = this;
        }

        /// <summary>
        /// Main Initialize function.
        /// </summary>
        protected override void Initialize()
        {
            // Initialize graphics
            ScreenHandler.Width = Root.Settings.DefaultScreenWidth;
            ScreenHandler.Height = Root.Settings.DefaultScreenHeight;
            ScreenHandler.MaxFramesPerSecond = Root.Settings.DefaultMaxFPS;
            ScreenHandler.UseVsync = true;
            ScreenHandler.ApplyChanges();

            // Initialize Globals' Texture2Ds
            Texture2D pixelTexture = new(GraphicsDevice, 1, 1);
            pixelTexture.SetData(new[] { Color.White });
            Root.PixelTexture = pixelTexture;
            Texture2D transparentTexture = new(GraphicsDevice, 1, 1);
            transparentTexture.SetData(new[] { Color.Transparent });
            Root.TransparentTexture = transparentTexture;

            // Set physics world settings
            Genbox.VelcroPhysics.Settings.PositionIterations = Root.Settings.Physics.PositionIterations;
            Genbox.VelcroPhysics.Settings.VelocityIterations = Root.Settings.Physics.VelocityIterations;
            Genbox.VelcroPhysics.Settings.MaxTranslation = Root.Settings.Physics.MaxTranslation;

            // Set other stuff
            SceneHandler.InitializeScenesAction = InitializeScenes;

            // ---------
            base.Initialize();

            OnInitializing.Invoke();
        }

        public delegate void OnInitializingHandler();
        public OnInitializingHandler OnInitializing { get; set; }

        /// <summary>
        /// Main LoadContent function.
        /// </summary>
        protected override void LoadContent()
        {
            // Load Globals' ContentManager and SpriteBatch
            Root.Content = Content;
            Root.SpriteBatch = new SpriteBatch(GraphicsDevice);

            // Load Globals' SpriteFonts
            Root.LogFont = Root.Content.Load<SpriteFont>("Fonts/LogFont");

            OnLoadingContent.Invoke();
            OnContentLoaded.Invoke();
        }

        public delegate void OnLoadingContentHandler();
        public OnLoadingContentHandler OnLoadingContent { get; set; }

        public delegate void OnContentLoadedHandler();
        public OnContentLoadedHandler OnContentLoaded { get; set; }

        /// <summary>
        /// Main UnloadContent function.
        /// </summary>
        protected override void UnloadContent()
        {
            AudioHandler.Dispose();
            foreach (object thing in SceneHandler.Everything.SelectMany(scene => scene.Things))
            {
                if (thing is IDisposable disposable)
                {
                    disposable.Dispose();
                }
            }

            // Unload Globals' ContentManager and SpriteBatch
            Root.Content.Unload();
            Root.SpriteBatch.Dispose();
            Root.SpriteBatch = null;
            Root.Content = null;

            OnUnloadingContent.Invoke();
        }

        public delegate void OnUnloadingContentHandler();
        public OnUnloadingContentHandler OnUnloadingContent { get; set; }

        /// <summary>
        /// Main Start function. This function is called in the first Update() function call, before anything gets updated.
        /// </summary>
        private void Start()
        {
            StartFunctions.StartEverything(SceneHandler);
        }

        private float _elapsedTimeForFixedUpdate = 0f;
        /// <summary>
        /// Main Update function.
        /// </summary>
        protected override void Update(GameTime gameTime)
        {
            if (IsFirstUpdateCall)
            {
                _elapsedTimeForFixedUpdate = 0f;

                Start();

                IsFirstUpdateCall = false;
            }

            TimeSpan scaledElapsedGameTime = new((long)(gameTime.ElapsedGameTime.Ticks * TimeScale));

            float deltaTime = (float)scaledElapsedGameTime.TotalSeconds;
            _elapsedTimeForFixedUpdate += deltaTime;

            while (_elapsedTimeForFixedUpdate >= FixedUpdateTimeStep) // Fixed update
            {
                // Calculate the time between every frame update
                TimeSpan correctedElapsedGameTime = new((long)(scaledElapsedGameTime.Ticks * (FixedUpdateTimeStep / deltaTime)));
                GameTime correctedGameTime = new(gameTime.TotalGameTime, correctedElapsedGameTime);

                // Calculate the time between every fixedupdate call (use elapsedTimeForFixedUpdate)
                TimeSpan fixedElapsedGameTime = new((long)(_elapsedTimeForFixedUpdate * TimeSpan.TicksPerSecond));
                GameTime fixedGameTime = new(gameTime.TotalGameTime, fixedElapsedGameTime);

                FixedUpdate(correctedGameTime, fixedGameTime);
                _elapsedTimeForFixedUpdate -= FixedUpdateTimeStep;
            }

            InterpolationAlpha = MathHelper.Clamp(_elapsedTimeForFixedUpdate / FixedUpdateTimeStep, 0f, 1f);

            UpdateFunctions.HandleUpdate(SceneHandler, gameTime);

            TargetElapsedTime = TimeSpan.FromSeconds(UpdateTimeStep); // Set the fixed time step

            // ---------
            base.Update(gameTime);
        }

        /// <summary>
        /// Main FixedUpdate function.
        /// </summary>
        private void FixedUpdate(GameTime gameTime, GameTime fixedGameTime)
        {
            PhysicsWorld.Gravity = Root.Settings.PhysicsWorldGravity;
            PhysicsWorld.Step(FixedUpdateTimeStep);
            UpdateFunctions.HandleFixedUpdate(SceneHandler, gameTime, fixedGameTime);
        }

        /// <summary>
        /// Main Draw function. This function is called in the Root.cs Draw() function.
        /// </summary>
        protected override void Draw(GameTime gameTime)
        {
            double fps = 1 / gameTime.ElapsedGameTime.TotalSeconds;
            ScreenHandler.UpdateFramesPerSecond(fps);

            GraphicsDevice.Clear(new Color(202, 228, 229, 255));

            DrawFunctions.DrawSprites(ScreenHandler, SceneHandler, gameTime);
            DrawFunctions.DrawDraws(ScreenHandler, SceneHandler, gameTime);

            if (GridSettings.Enabled)
            {
                DrawGrid(GridSettings);
            }

            // ---------
            base.Draw(gameTime);
        }

        private void DrawGrid(GridSettings gridSettings)
        {
            if (gridSettings.Camera == null)
            {
                Camera2D firstActiveCamera = ActiveScene.ActiveCameras.FirstOrDefault();
                if (firstActiveCamera == null)
                {
                    LogManager.DoConsoleLog("No camera found to draw the grid!", LogLevel.Warning);
                    return;
                }

                gridSettings.Camera = firstActiveCamera;
            }

            DrawFunctions.DrawGrid(gridSettings);
        }

        /// <summary>
        /// Initialize all the scenes.
        /// </summary>
        private void InitializeScenes()
        {
            StartOver();

            OnSceneInitialization.Invoke();
        }

        public delegate void OnSceneInitializationHandler();
        public OnSceneInitializationHandler OnSceneInitialization { get; set; }

        /// <summary>
        /// Start the engine over. This is, for example, called at the start of scene initialization.
        /// </summary>
        private void StartOver()
        {
            SceneHandler.Reset();
            GridSettings.Camera = null;

            PhysicsWorld = new(Root.Settings.PhysicsWorldGravity);
            LogManager.ClearShownScreenLogs();

            IsFirstUpdateCall = true;
        }
    }
}