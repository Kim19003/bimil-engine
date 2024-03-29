﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using BimilEngine.Source.Engine;
using BimilEngine.Source.GameLogic;

namespace BimilEngine
{
    public class Game1 : Game
    {
        /// <summary>
        /// The physics hertz of the game.
        /// </summary>
        public const float PHYSICS_HERTZ = 120f;

        /// <summary>
        /// Is this the first update call? Set this to true, if you want to do something only once in the first update call.
        /// </summary>
        public static bool IsFirstUpdateCall { get; set; } = true;
        /// <summary>
        /// Is this the first draw call? Set this to true, if you want to do something only once in the first draw call.
        /// </summary>
        public static bool IsFirstDrawCall { get; set; } = true;

        /// <summary>
        /// The fixed update time step of the game.
        /// </summary>
        public static float FixedUpdateTimeStep => 1 / PHYSICS_HERTZ;
        /// <summary>
        /// The update time step of the game.
        /// </summary>
        public static float UpdateTimeStep => Environment2D.ScreenHandler.MaxFramesPerSecond > 0f
            ? 1f / Environment2D.ScreenHandler.MaxFramesPerSecond
            : 0.00001f;

        public Game1()
        {
            Globals.Graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            Globals.Graphics.SynchronizeWithVerticalRetrace = false; // Disable Vsync for now. Use ScreenHandler.UseVsync to set it on/off.
            IsFixedTimeStep = true; // Use fixed time step. This is used for the FPS limiting.
            TargetElapsedTime = TimeSpan.FromSeconds(UpdateTimeStep); // Set the fixed time step with UpdateTimeStep (basically the FPS).
        }

        protected override void Initialize()
        {
            Environment2D.Initialize(GraphicsDevice);

            // ---------
            base.Initialize();
        }

        protected override void LoadContent()
        {
            Environment2D.LoadContent(Content, GraphicsDevice);
        }

        protected override void UnloadContent()
        {
            Environment2D.UnloadContent();
        }

        float interpolationAlpha = 0f;
        float elapsedTimeForFixedUpdate = 0f;
        protected override void Update(GameTime gameTime)
        {
            if (IsFirstUpdateCall)
            {
                elapsedTimeForFixedUpdate = 0f;

                Environment2D.Start();

                IsFirstUpdateCall = false;
            }

            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            elapsedTimeForFixedUpdate += deltaTime;

            while (elapsedTimeForFixedUpdate >= FixedUpdateTimeStep) // Fixed update
            {
                // Calculate the time between every frame update
                TimeSpan correctedElapsedGameTime = new((long)(gameTime.ElapsedGameTime.Ticks * (FixedUpdateTimeStep / deltaTime)));
                GameTime correctedGameTime = new(gameTime.TotalGameTime, correctedElapsedGameTime);
                // Calculate the time between every fixedupdate call (use elapsedTimeForFixedUpdate)
                TimeSpan fixedElapsedGameTime = new((long)(elapsedTimeForFixedUpdate * TimeSpan.TicksPerSecond));
                GameTime fixedGameTime = new(gameTime.TotalGameTime, fixedElapsedGameTime);

                Environment2D.FixedUpdate(correctedGameTime, fixedGameTime);
                elapsedTimeForFixedUpdate -= FixedUpdateTimeStep;
            }

            interpolationAlpha = MathHelper.Clamp(elapsedTimeForFixedUpdate / FixedUpdateTimeStep, 0.0f, 1.0f);

            Environment2D.Update(gameTime);

            TargetElapsedTime = TimeSpan.FromSeconds(UpdateTimeStep); // Set the fixed time step

            // ---------
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            if (IsFirstDrawCall)
            {
                // TODO: Implement your own logic here
                
                IsFirstDrawCall = false;
            }

            GraphicsDevice.Clear(new Color(202, 228, 229, 255));

#warning interpolationAlpha might be calculated incorrectly, use it with caution!
            // float interpolationAlpha = MathHelper.Clamp(elapsedTimeForFixedUpdate / FixedUpdateTimeStep, 0.0f, 1.0f);
            
            Environment2D.Draw(gameTime, interpolationAlpha);

            // ---------
            base.Draw(gameTime);
        }
    }
}