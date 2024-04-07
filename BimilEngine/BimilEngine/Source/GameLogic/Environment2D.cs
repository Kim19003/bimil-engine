using System.Linq;
using Microsoft.Xna.Framework;
using BimilEngine.Source.GameLogic.Sprites;
using BimilEngine.Source.GameLogic.Models;
using BimilEngine.Source.Engine.Handlers;
using BimilEngine.Source.Engine.Other;
using BimilEngine.Source.Engine.Functions;
using Genbox.VelcroPhysics.Dynamics;
using BimilEngine.Source.Engine.Models;
using Microsoft.Xna.Framework.Graphics;
using BimilEngine.Source.Engine.Objects;
using BimilEngine.Source.Engine;
using Microsoft.Xna.Framework.Content;
using BimilEngine.Source.Engine.Managers;
using Genbox.VelcroPhysics;
using BimilEngine.Source.GameLogic.Gadgets;
using System;

namespace BimilEngine.Source.GameLogic
{
    public static class Environment2D
    {
        /// <summary>
        /// Default screen width.
        /// </summary>
        public const int DEFAULT_SCREEN_WIDTH = 1280;
        /// <summary>
        /// Default screen height.
        /// </summary>
        public const int DEFAULT_SCREEN_HEIGHT = 720;
        /// <summary>
        /// Default max frames per second.
        /// </summary>
        public const int DEFAULT_MAX_FPS = 999;
        /// <summary>
        /// Handler, that handles everything screen related.
        /// </summary>
        public static readonly ScreenHandler ScreenHandler = new();
        /// <summary>
        /// Handler, that handles all the audio.
        /// </summary>
        public static readonly AudioHandler AudioHandler = new();
        /// <summary>
        /// Grid settings used in the grid drawing. Set Enabled to true to enable the grid.
        /// </summary>
        public static readonly GridSettings GridSettings = new();

        /// <summary>
        /// The physics world (this handles all the physics).
        /// </summary>
        public static World PhysicsWorld { get; private set; }
        /// <summary>
        /// Gravity used in the physics world.
        /// </summary>
        public static Vector2 PhysicsWorldGravity { get; set; } = new(0, 147f); // Default is usually (0, 9.81f)

        /// <summary>
        /// The active scene.
        /// </summary>
        public static Scene2D ActiveScene => _sceneHandler.ActiveScene;

        /// <summary>
        /// Handler, that handles all the scenes.
        /// </summary>
        private static SceneHandler _sceneHandler;

        private static bool _initializeCalled = false;
        /// <summary>
        /// Main Initialize function. This function is called in the Game1.cs Initialize() function.
        /// </summary>
        public static void Initialize(GraphicsDevice graphicsDevice)
        {
            if (_initializeCalled) LogManager.DoConsoleLog("Multiple calls on Initialize().", LogLevel.Warning);

            // Initialize graphics
            ScreenHandler.Width = DEFAULT_SCREEN_WIDTH;
            ScreenHandler.Height = DEFAULT_SCREEN_HEIGHT;
            ScreenHandler.MaxFramesPerSecond = DEFAULT_MAX_FPS;
            ScreenHandler.UseVsync = true;
            ScreenHandler.ApplyChanges();

            // Initialize Globals' Texture2Ds
            Texture2D pixelTexture = new(graphicsDevice, 1, 1);
            pixelTexture.SetData(new[] { Color.White });
            Globals.PixelTexture = pixelTexture;
            Texture2D transparentTexture = new(graphicsDevice, 1, 1);
            transparentTexture.SetData(new[] { Color.Transparent });
            Globals.TransparentTexture = transparentTexture;

            // Set physics world settings
            Settings.PositionIterations = 6;
            Settings.VelocityIterations = 16;
            Settings.MaxTranslation = 100f;

            _initializeCalled = true;
        }

        private static bool _loadContentCalled = false;
        /// <summary>
        /// Main LoadContent function. This function is called in the Game1.cs LoadContent() function.
        /// </summary>
        public static void LoadContent(ContentManager contentManager, GraphicsDevice graphicsDevice)
        {
            if (_loadContentCalled) LogManager.DoConsoleLog("Multiple calls on LoadContent().", LogLevel.Warning);

            // Load Globals' ContentManager and SpriteBatch
            Globals.Content = contentManager;
            Globals.SpriteBatch = new SpriteBatch(graphicsDevice);

            // Load Globals' SpriteFonts
            Globals.LogFont = Globals.Content.Load<SpriteFont>("Fonts/LogFont");

            // Load textures
            Globals.TextureBatch = new()
            {
                { "Square Head Idle Gun Looking Right", Globals.Content.Load<Texture2D>("Textures/Square Head Idle Gun Looking Right") },
                { "Square Head Idle Gun Looking Right Hands Up", Globals.Content.Load<Texture2D>("Textures/Square Head Idle Gun Looking Right Hands Up") },
                { "Square Head Idle Gun Looking Left", Globals.Content.Load<Texture2D>("Textures/Square Head Idle Gun Looking Left") },
                { "Square Head Idle Gun Looking Left Hands Up", Globals.Content.Load<Texture2D>("Textures/Square Head Idle Gun Looking Left Hands Up") },
                { "Grass P Mini", Globals.Content.Load<Texture2D>("Textures/Grass P Mini") },
                { "Lava Middle", Globals.Content.Load<Texture2D>("Textures/Lava Middle") },
                { "Wall Middle", Globals.Content.Load<Texture2D>("Textures/Wall Middle") },
            };

            // Load the starting scene
            LoadScene("Main");

            _loadContentCalled = true;
        }

        private static bool _unloadContentCalled = false;
        /// <summary>
        /// Main UnloadContent function. This function is called in the Game1.cs UnloadContent() function.
        /// </summary>
        public static void UnloadContent()
        {
            if (_unloadContentCalled) LogManager.DoConsoleLog("Multiple calls on UnloadContent().", LogLevel.Warning);

            AudioHandler.Dispose();
            foreach (object thing in _sceneHandler.Everything.SelectMany(scene => scene.Things))
            {
                if (thing is IDisposable disposable)
                {
                    disposable.Dispose();
                }
            }

            // Unload Globals' ContentManager and SpriteBatch
            Globals.Content.Unload();
            Globals.SpriteBatch.Dispose();
            Globals.SpriteBatch = null;
            Globals.Content = null;

            _unloadContentCalled = true;
        }

        /// <summary>
        /// Main Start function. This function is called before the first (Fixed)Update() function call.
        /// </summary>
        public static void Start()
        {
            StartFunctions.StartEverything(_sceneHandler);
        }

        /// <summary>
        /// Main Update function. This function is called in the Game1.cs Update() function.
        /// </summary>
        public static void Update(GameTime gameTime)
        {
            UpdateFunctions.HandleUpdate(_sceneHandler, gameTime);

            // PhysicsWorld.Step((float)gameTime.ElapsedGameTime.TotalSeconds);
        }

        /// <summary>
        /// Main FixedUpdate function. This function is called in the Game1.cs Update() function.
        /// </summary>
        public static void FixedUpdate(GameTime gameTime, GameTime fixedGameTime)
        {
            PhysicsWorld.Gravity = PhysicsWorldGravity;
            PhysicsWorld.Step(Game1.FixedUpdateTimeStep);
            UpdateFunctions.HandleFixedUpdate(_sceneHandler, gameTime, fixedGameTime);
        }

        /// <summary>
        /// Main Draw function. This function is called in the Game1.cs Draw() function.
        /// </summary>
        public static void Draw(GameTime gameTime)
        {
            double fps = 1 / gameTime.ElapsedGameTime.TotalSeconds;
            ScreenHandler.UpdateFramesPerSecond(fps);

            DrawFunctions.DrawSprites(_sceneHandler, gameTime);
            DrawFunctions.DrawDraws(_sceneHandler, gameTime);

            if (GridSettings.Enabled)
            {
                DrawGrid(GridSettings);
            }
        }

        private static void DrawGrid(GridSettings gridSettings)
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
        /// Create the scenes and add them to the scene handler.
        /// </summary>
        private static void CreateScenes()
        {
            // Reinstantiate things
            ReinstantiateThings();

            // Create the main scene
            Scene2D main = new("Main");
            main.AddGadgets(new object[]
            {
                new GameController(Vector2.Zero, Vector2.Zero, 0, "Game Controller"),
                new Camera2D(Vector2.Zero, new(ScreenHandler.Width, ScreenHandler.Height), 0, "Main Camera")
                {
                    Depth = 2,
                    SortMode = SpriteSortMode.Deferred,
                    BlendState = BlendState.AlphaBlend,
                    SamplerState = SamplerState.PointClamp,
                },
                // new Camera2D(new(ScreenHandler.ScreenWidth / 2, 0), new(ScreenHandler.ScreenWidth / 2, ScreenHandler.ScreenHeight), "Second Camera")
                // {
                //     Depth = 2,
                //     SortMode = SpriteSortMode.Deferred,
                //     BlendState = BlendState.AlphaBlend,
                //     SamplerState = SamplerState.PointClamp,
                // },
            });

            Vector2 textureSize = new(32, 32);

            Camera2D mainCamera = (Camera2D)main.Gadgets.FirstOrDefault(s => s is Camera2D camera && camera.Name == "Main Camera");
            Rectangle mainCameraBounds = mainCamera.WorldPointBounds;

            main.AddSprites(new object[]
            {
                new Player("Square Head Idle Gun Looking Right", textureSize.MultiplyFollowing(new(0, -2)),
                    new(1), new(0.5f, 1.8f), mainCamera.CameraLevel, "Player", SpriteTags.Player),

                // new Wall("Grass P Mini", textureSize.Multiply(new(0, 0)),
                //     new(1), new(1, 1), mainCamera.CameraLevel, "Wall 3", SpriteTags.Wall),

                // new Wall("Grass P Mini", textureSize.Multiply(new(-2, 0)),
                //     new(1), new(1, 1), mainCamera.CameraLevel, "Wall 4", SpriteTags.Wall),

                // new Wall("Grass P Mini", textureSize.Multiply(new(2, 0)),
                //     new(1), new(1, 1), mainCamera.CameraLevel, "Wall 5", SpriteTags.Wall),
                    
                // new Wall("Pixels/Black Pixel", new(mainCameraBounds.X, mainCameraBounds.Y + mainCameraBounds.Height - 50), new(mainCameraBounds.Width * 10, 100), mainCamera.CameraLevel, "Floor", SpriteTags.Wall),
            });

            Vector2 previousPosition = new(0, 0);
            Random random = new();
            for (int i = 0; i < 50; i++)
            {
                Vector2 newPosition;
                if (i == 0)
                    newPosition = new(0, 0);
                else
                    newPosition = new Vector2(previousPosition.X + 2, previousPosition.Y - random.Next(-1, 2));
                Wall wall = new("Grass P Mini", textureSize.MultiplyFollowing(newPosition),
                    new(1), new(1, 1), mainCamera.CameraLevel, $"Wall {i + 1}", SpriteTags.Wall);
                main.AddSprite(wall);
                previousPosition = newPosition;
            }

            // Camera2D secondCamera = (Camera2D)main.Gadgets.FirstOrDefault(s => s is Camera2D camera && camera.Name == "Second Camera");

            main.AddActiveCameras(new Camera2D[]
            {
                mainCamera,
                // secondCamera,
            });

            main.AddOrUpdateDraws(new Draw[]
            {
                // new(mainCamera, Color.Red, lineThickness: 5),
                // new(secondCamera, Color.Orange) { CameraLevel = secondCamera.Level },

                // new(((Player)main.Sprites.FirstOrDefault(s => s is Player)).Rigidbody2D.Body, Color.Green, cameraLevel: mainCamera.CameraLevel, lineThickness: 1f),
                // new(((Wall)main.Sprites.FirstOrDefault(s => ((Transform2D)s).Name == "Wall 3")).Rigidbody2D.Body, Color.Green, cameraLevel: mainCamera.CameraLevel, lineThickness: 1f),
                // new(((Wall)main.Sprites.FirstOrDefault(s => ((Transform2D)s).Name == "Wall 4")).Rigidbody2D.Body, Color.Green, cameraLevel: mainCamera.CameraLevel, lineThickness: 1f),
                // new(((Wall)main.Sprites.FirstOrDefault(s => ((Transform2D)s).Name == "Wall 5")).Rigidbody2D.Body, Color.Green, cameraLevel: mainCamera.CameraLevel, lineThickness: 1f),
            });

            // // Create the second scene
            // Scene2D second = new("Second");
            // second.AddGadgets(new object[]
            // {
            //     new Camera2D(Vector2.Zero, Vector2.One, "Main Camera") { PixelsPerUnit = cameraPixelsPerUnit, ZoomValue = fixedCameraPixelPerfectZoomValue },
            // });
            // second.AddSprites(new object[]
            // {
            //     new Player("Lava Middle", new(300, DefaultScreenHeight - 250), new(100, 100), "Player", SpriteTags.Player),
            //     new Wall("Wall Middle", new(600, DefaultScreenHeight - 600), new(100, 400), "Wall", SpriteTags.Wall),
            // });

            // Add the scenes to the environment
            _sceneHandler.AddScene(main);
            // sceneHandler.AddScene(second);
        }

        private static void ReinstantiateThings()
        {
            _sceneHandler = new();
            GridSettings.Camera = null;

            PhysicsWorld = new(PhysicsWorldGravity);
            LogManager.ClearShownScreenLogs();

            Game1.IsFirstUpdateCall = true;
            Game1.IsFirstDrawCall = true;
        }

        /// <summary>
        /// Load a scene by name. If resetScenes is true, all scenes will be reset before loading the scene.
        /// </summary>
        public static void LoadScene(string sceneName, bool resetScenes = true)
        {
            if (_sceneHandler != null && _sceneHandler.Scenes.Any() && !_sceneHandler.Scenes.Any(scene => scene.Name == sceneName))
                throw new System.Exception($"Scene with name {sceneName} does not exist");

            if (resetScenes)
                CreateScenes(); // This creates/resets the scenes

            _sceneHandler.SetActiveScene(sceneName);
        }

        /// <summary>
        /// Load a scene by reference. If resetScenes is true, all scenes will be reset before loading the scene.
        /// </summary>
        public static void LoadScene(Scene2D scene, bool resetScenes = true)
        {
            if (_sceneHandler != null && _sceneHandler.Scenes.Any() && !_sceneHandler.Scenes.Any(s => s == scene))
                throw new System.Exception($"Scene with name {scene.Name} does not exist");

            if (resetScenes)
                CreateScenes(); // This creates/resets the scenes
                
            _sceneHandler.SetActiveScene(scene);
        }
    }
}