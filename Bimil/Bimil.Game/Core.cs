using System.Collections.Generic;
using Bimil.Engine;
using Bimil.Engine.Objects.Bases;
using Bimil.Game.Scenes;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace Bimil.Game
{
    public static class Core
    {
        /// <summary>
        /// Build the game core.
        /// </summary>
        public static void Build()
        {
            // Set settings
            Root.Settings.PhysicsHertz = 120f;
            Root.Settings.DefaultScreenWidth = 1280;
            Root.Settings.DefaultScreenHeight = 720;
            Root.Settings.DefaultMaxFPS = 999;
            Root.Settings.PhysicsWorldGravity = new(0, 200f);
            Root.Settings.Physics.PositionIterations = 6;
            Root.Settings.Physics.VelocityIterations = 16;
            Root.Settings.Physics.MaxTranslation = 200f;

            // Subscribe to events
            Root.Core.OnInitializing += Initialize;
            Root.Core.OnLoadingContent += LoadContent;
            Root.Core.OnContentLoaded += ContentLoaded;
            Root.Core.OnUnloadingContent += UnloadContent;
            Root.Core.OnSceneInitialization += InitializeScenes;
        }

        /// <summary>
        /// Initialize things here.
        /// </summary>
        private static void Initialize()
        {
            
        }

        /// <summary>
        /// Load content here.
        /// </summary>
        private static void LoadContent()
        {
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
                { "Black Pixel", Root.Content.Load<Texture2D>("Textures/Pixels/Black Pixel") },
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
            Root.Core.SceneHandler.LoadScene(nameof(Main));
        }

        /// <summary>
        /// Unload content here.
        /// </summary>
        private static void UnloadContent()
        {
            
        }

        /// <summary>
        /// Initialize the scenes here.
        /// </summary>
        private static void InitializeScenes()
        {
            HashSet<Scene2D> scenes = new()
            {
                new Main(),
                new Secondary(),
            };

            // ---------
            foreach (Scene2D scene in scenes)
            {
                Root.Core.SceneHandler.AddScene(scene);
            }
        }
    }
}