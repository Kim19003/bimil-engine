using System.Collections.Generic;
using System.Linq;
using Genbox.VelcroPhysics.Dynamics;
using Microsoft.Xna.Framework;
using BimilEngine.Source.Engine.Handlers;
using BimilEngine.Source.Engine.Interfaces;
using BimilEngine.Source.Engine.Objects.Bases;
using BimilEngine.Source.GameLogic;
using BimilEngine.Source.Engine.Models;

namespace BimilEngine.Source.Engine.Functions
{
    public static class UpdateFunctions
    {
        public static void HandleFixedUpdate(SceneHandler sceneHandler, GameTime gameTime, GameTime fixedGameTime)
        {
            // TODO: Add fixed update logic here
            FixedUpdateGadgets(sceneHandler, gameTime, fixedGameTime);
            FixedUpdateSprites(sceneHandler, gameTime, fixedGameTime);
        }

        public static void HandleUpdate(SceneHandler sceneHandler, GameTime gameTime)
        {
            // TODO: Add update logic here
            UpdateGadgets(sceneHandler, gameTime);
            UpdateSprites(sceneHandler, gameTime);
        }

        public static void UpdateGadgets(SceneHandler sceneHandler, GameTime gameTime)
        {
            // Get the gadgets from the active scene
            IReadOnlyCollection<object> gadgets = sceneHandler.ActiveScene.Gadgets;

            if (gadgets == null || !gadgets.Any()) return;

            // Update the gadgets
            foreach (object gadget in gadgets)
            {
                if (gadget == null || gadget is not IUpdatable) continue;

                ((Gadget2D)gadget).Update(gameTime);
            }
        }

        public static void FixedUpdateGadgets(SceneHandler sceneHandler, GameTime gameTime, GameTime fixedGameTime)
        {
            // Get the gadgets from the active scene
            IReadOnlyCollection<object> gadgets = sceneHandler.ActiveScene.Gadgets;

            if (gadgets == null || !gadgets.Any()) return;

            // Update the gadgets
            foreach (object gadget in gadgets)
            {
                if (gadget == null || gadget is not IUpdatable) continue;

                ((Gadget2D)gadget).FixedUpdate(gameTime, fixedGameTime);
            }
        }

        public static void UpdateSprites(SceneHandler sceneHandler, GameTime gameTime)
        {
            // Get the sprites from the active scene
            IReadOnlyCollection<object> sprites = sceneHandler.ActiveScene.Sprites;

            if (sprites == null || !sprites.Any()) return;

            // Update the sprites
            foreach (object sprite in sprites)
            {
                if (sprite == null || sprite is not IUpdatable) continue;

                ((IUpdatable)sprite).Update(gameTime);
            }
        }

        public static void FixedUpdateSprites(SceneHandler sceneHandler, GameTime gameTime, GameTime fixedGameTime)
        {
            // Get the sprites from the active scene
            IReadOnlyCollection<object> sprites = sceneHandler.ActiveScene.Sprites;

            if (sprites == null || !sprites.Any()) return;

            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Update the sprites
            foreach (object sprite in sprites)
            {
                if (sprite == null || sprite is not IUpdatable) continue;

                ((IUpdatable)sprite).FixedUpdate(gameTime, fixedGameTime);

                // Update the sprite values if it is a physics sprite
                if (sprite is PhysicsSprite2D physicsSprite)
                {
                    float interpolationAlpha = MathHelper.Clamp((float)fixedGameTime.ElapsedGameTime.TotalSeconds / Game1.FixedUpdateTimeStep, 0.0f, 1.0f);

                    physicsSprite.LastPosition = physicsSprite.Position;
                    physicsSprite.LastRotation = physicsSprite.Rotation;

                    if (physicsSprite.Rigidbody2D.Interpolation == RigidbodyInterpolation2D.Interpolate)
                    {
                        // Interpolate position and rotation
                        Vector2 position = Vector2.Lerp(physicsSprite.LastPosition, physicsSprite.Rigidbody2D.Body.Position, interpolationAlpha);
                        physicsSprite.Position = position;
                        float rotation = MathHelper.Lerp(physicsSprite.LastRotation, physicsSprite.Rigidbody2D.Body.Rotation, interpolationAlpha);
                        physicsSprite.Rotation = rotation;
                    }
                    else
                    {
                        physicsSprite.Position = physicsSprite.Rigidbody2D.Body.Position;
                        physicsSprite.Rotation = physicsSprite.Rigidbody2D.Body.Rotation;
                    }
                }
            }
        }
    }
}