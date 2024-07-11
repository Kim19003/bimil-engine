using Bimil.Engine;
using Bimil.Engine.Models;
using Bimil.Engine.Objects;
using Bimil.Game.Gadgets;
using Bimil.Game.Models;
using Bimil.Game.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using System;
using System.Linq;

namespace Bimil.Game
{
    public static class GameCore
    {
        /// <summary>
        /// Build the game core.
        /// </summary>
        public static void Build()
        {
            // Set settings
            Settings.PhysicsHertz = 120f;
            Settings.DefaultScreenWidth = 1280;
            Settings.DefaultScreenHeight = 720;
            Settings.DefaultMaxFPS = 999;
            Settings.PhysicsWorldGravity = new(0, 200f);
            Settings.Physics.PositionIterations = 6;
            Settings.Physics.VelocityIterations = 16;
            Settings.Physics.MaxTranslation = 200f;

            // Subscribe to events
            Root.EngineCore.OnInitializing += Initialize;
            Root.EngineCore.OnLoadingContent += LoadContent;
            Root.EngineCore.OnContentLoaded += ContentLoaded;
            Root.EngineCore.OnUnloadingContent += UnloadContent;
            Root.EngineCore.OnSceneCreation += CreateScenes;
        }

        /// <summary>
        /// Initialize things here.
        /// </summary>
        private static void Initialize()
        {
            // * [OPTIONAL] TODO: Add your own initializing logic here...
        }

        /// <summary>
        /// Load content here.
        /// </summary>
        private static void LoadContent()
        {
            // * [OPTIONAL] TODO: Add your own load content logic here...

            // Load textures
            Root.TextureBatch = new()
            {
                { "Square Head Idle Gun Looking Right", Root.Content.Load<Texture2D>("Textures/Square Head Idle Gun Looking Right") },
                { "Square Head Idle Gun Looking Right Hands Up", Root.Content.Load<Texture2D>("Textures/Square Head Idle Gun Looking Right Hands Up") },
                { "Square Head Idle Gun Looking Left", Root.Content.Load<Texture2D>("Textures/Square Head Idle Gun Looking Left") },
                { "Square Head Idle Gun Looking Left Hands Up", Root.Content.Load<Texture2D>("Textures/Square Head Idle Gun Looking Left Hands Up") },
                { "Grass P Mini", Root.Content.Load<Texture2D>("Textures/Grass P Mini") },
                { "Lava Middle", Root.Content.Load<Texture2D>("Textures/Lava Middle") },
                { "Wall Middle", Root.Content.Load<Texture2D>("Textures/Wall Middle") },
            };

            // Load songs
            Root.SongBatch = new()
            {
                { "Main Theme", Root.Content.Load<Song>("Audio/theme") },
            };

            // Load sound effects
            Root.SoundEffectBatch = new()
            {
                { "Jump", Root.Content.Load<SoundEffect>("Audio/jump") },
            };
        }

        /// <summary>
        /// Do things after content is loaded here.
        /// </summary>
        private static void ContentLoaded()
        {
            // * [OPTIONAL] TODO: Add your own content loaded logic here...

            Root.EngineCore.SceneHandler.LoadScene("Main");
        }

        /// <summary>
        /// Unload content here.
        /// </summary>
        private static void UnloadContent()
        {
            // * [OPTIONAL] TODO: Add your own unload content logic here...
        }

        /// <summary>
        /// Create the scenes here.
        /// </summary>
        private static void CreateScenes()
        {
            // ! [NECESSARY] TODO: Add your own scene creation logic here...

            // Create the main scene
            Scene2D main = new("Main");
            main.AddGadgets(new object[]
            {
                new GameController()
                {
                    Name = "Game Controller",
                },
                new Camera2D(Vector2.Zero)
                {
                    Scale = new(Root.EngineCore.ScreenHandler.Width, Root.EngineCore.ScreenHandler.Height),
                    CameraLevel = 0,
                    Name = "Main Camera",
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

            Camera2D mainCamera = (Camera2D)main.Gadgets.FirstOrDefault(s => s is Camera2D camera && camera.Name == "Main Camera");
            Rectangle mainCameraBounds = mainCamera.WorldPointBounds;

            main.AddSprites(new object[]
            {
                new Player("Square Head Idle Gun Looking Right", new(0, -2))
                {
                    Scale = new(1),
                    Rotation = 0,
                    PhysicsScale = new(0.5f, 1.8f),
                    CameraLevel = mainCamera.CameraLevel,
                    Name = "Player",
                    Tag = SpriteTags.PLAYER,
                },

                // new Wall("Grass P Mini", textureSize.Multiply(new(0, 0)),
                //     new(1), new(1, 1), mainCamera.CameraLevel, "Wall 3", SpriteTags.WALL),

                // new Wall("Grass P Mini", textureSize.Multiply(new(-2, 0)),
                //     new(1), new(1, 1), mainCamera.CameraLevel, "Wall 4", SpriteTags.WALL),

                // new Wall("Grass P Mini", textureSize.Multiply(new(2, 0)),
                //     new(1), new(1, 1), mainCamera.CameraLevel, "Wall 5", SpriteTags.WALL),
                    
                // new Wall("Pixels/Black Pixel", new(mainCameraBounds.X, mainCameraBounds.Y + mainCameraBounds.Height - 50), new(mainCameraBounds.Width * 10, 100), mainCamera.CameraLevel, "Floor", SpriteTags.WALL),
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
                Wall wall = new("Grass P Mini", newPosition,
                    new(1), new(1, 1), mainCamera.CameraLevel, $"Wall {i + 1}", SpriteTags.WALL);
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
            Root.EngineCore.SceneHandler.AddScene(main);
            // sceneHandler.AddScene(second);
        }
    }
}