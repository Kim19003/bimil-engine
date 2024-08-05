using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Bimil.Engine.Interfaces;
using Bimil.Engine.Models;
using IDrawable = Bimil.Engine.Interfaces.IDrawable;
using Genbox.VelcroPhysics.Dynamics;
using Genbox.VelcroPhysics.Collision.ContactSystem;
using System.Collections.Generic;
using Bimil.Engine.Handlers;
using System;
using Bimil.Engine.Managers;

namespace Bimil.Engine.Objects.Bases
{
    public abstract class PhysicsSprite2D : Transform2D, IStartable, IDrawable, IUpdatable, IDestroyable, IPhysics2D
    {
        /// <summary>
        /// Texture of the sprite.
        /// </summary>
        public Texture2D Texture { get; set; } = null;
        /// <summary>
        /// Sprite effects of the sprite.
        /// </summary>
        public SpriteEffects SpriteEffects { get; set; } = SpriteEffects.None;
        /// <summary>
        /// Sorting layer of the sprite.
        /// </summary>
        public float SortingLayer { get; set; } = 0f;
        /// <summary>
        /// Physics scale of the transform.
        /// </summary>
        public Vector2 PhysicsScale { get; set; } = Vector2.One;
        /// <summary>
        /// Rigidbody2D of the sprite.
        /// </summary>
        public Rigidbody2D Rigidbody2D { get; set; } = null;

        /// <summary>
        /// Interpolation mode of the sprite.
        /// </summary>
        public Interpolation2D Interpolation { get; set; } = Interpolation2D.Interpolate;
        /// <summary>
        /// Interpolation hotspot of the sprite.
        /// </summary>
        public const float INTERPOLATION_HOTSPOT = 0.03f;
        /// <summary>
        /// Absolute position of the transform (current position multiplied by the texture size).
        /// </summary>
        public Vector2 AbsolutePosition
        {
            get
            {
                return Position * new Vector2(Texture.Width, Texture.Height);
            }
        }
        /// <summary>
        /// Last position of the transform, mainly used for position interpolation. You should not modify this value.
        /// </summary>
        public Vector2 LastAbsolutePosition { get; set; } = Vector2.Zero;
        /// <summary>
        /// Last rotation of the transform, mainly used for rotation interpolation. You should not modify this value.
        /// </summary>
        public float LastRotation { get; set; } = 0f;
        /// <summary>
        /// Calculates and returns the interpolated draw position of the sprite.
        /// </summary>
        public Vector2 InterpolatedDrawPosition
        {
            get
            {
                float interpolationAlpha = Root.Core.InterpolationAlpha;
                return Vector2.Lerp(LastAbsolutePosition, AbsolutePosition, interpolationAlpha);
            }
        }
        /// <summary>
        /// Calculates and returns the interpolated draw rotation of the sprite.
        /// </summary>
        public float InterpolatedDrawRotation
        {
            get
            {
                float interpolationAlpha = Root.Core.InterpolationAlpha;
                return MathHelper.Lerp(LastRotation, Rotation, interpolationAlpha);
            }
        }

        private readonly HashSet<(Fixture, Fixture, Contact)> _activeCollisions = new();

        public PhysicsSprite2D(string textureName, Scene2D associatedScene = null)
            : base(associatedScene)
        {
            Texture = !string.IsNullOrEmpty(textureName)
                ? Root.TextureBatch[textureName]
                : Root.TransparentTexture;
        }

        public virtual void Start()
        {
            // TODO: Add your start logic to-be-inherited here
        }

        public virtual void Update(GameTime gameTime)
        {
            // TODO: Add your update logic to-be-inherited here
        }

        public virtual void FixedUpdate(GameTime gameTime, GameTime fixedGameTime)
        {
            foreach (var activeCollision in _activeCollisions)
            {
                if (activeCollision.Item1.IsSensor)
                    OnTriggerStay2D(activeCollision.Item1, activeCollision.Item2, activeCollision.Item3);
                else
                    OnCollisionStay2D(activeCollision.Item1, activeCollision.Item2, activeCollision.Item3);
            }
        }

        public virtual void Draw(GameTime gameTime, AnimationHandler animationHandler = null)
        {
            Animation[] ongoingAnimations = animationHandler != null
                ? animationHandler.GetOngoingAnimations()
                : Array.Empty<Animation>();

            if (ongoingAnimations.Any())
            {
                HandleAnimations(ongoingAnimations, gameTime);
            }
            else
            {
                DrawTexture(Texture, gameTime, Interpolation);
            }
        }

        /// <summary>
        /// Used to store the ongoing texture draws of the animations.
        /// </summary>
        private readonly Dictionary<Animation, (Texture2D Texture, TimeSpan DrawTime)?> _ongoingTextureDraws = new();
        private void HandleAnimations(Animation[] ongoingAnimations, GameTime gameTime)
        {
            /*
                Clarification on the animations:
                
                - The animation is a sequence of textures and their durations.
                - The textures are drawn in a (sequential) order, and their durations are the time they are being drawn.
                - When the time of a texture's draw has finished, the next texture is drawn.
                - When the time of the last texture's draw has finished, the animation is finished.
                - The Repeat property of the animation determines if the animation should play again after it has finished.
                - Note: We are not drawing the textures frame by frame, but rather in a time-based manner.
            */

            if (ongoingAnimations.Length > 1)
                LogManager.DoConsoleLog("PUA: There are more than one ongoing animations.", LogLevel.Warning);

            TimeSpan totalElapsedTime = gameTime.TotalGameTime;

            foreach (Animation ongoingAnimation in ongoingAnimations)
            {
                for (int i = 0; i < ongoingAnimation.DuratedTextures.Count; i++)
                {
                    bool isLastIteration = i == ongoingAnimation.DuratedTextures.Count - 1;

                    Texture2D texture = ongoingAnimation.DuratedTextures.ElementAt(i).Texture; // The current texture
                    TimeSpan duration = ongoingAnimation.DuratedTextures.ElementAt(i).Duration; // Duration of the current texture

                    if (!_ongoingTextureDraws.ContainsKey(ongoingAnimation)) // If the whole animation is not yet started
                        _ongoingTextureDraws.Add(ongoingAnimation, (texture, totalElapsedTime)); // -> Start drawing the current texture
                    else if (_ongoingTextureDraws[ongoingAnimation] == null) // If the animation's previous texture draw has finished
                        _ongoingTextureDraws[ongoingAnimation] = (texture, totalElapsedTime); // -> Start drawing the current texture
                    else if (_ongoingTextureDraws[ongoingAnimation].Value.Texture != texture) // If there is an ongoing texture draw
                        continue; // -> Skip drawing the current texture

                    DrawTexture(texture, gameTime, Interpolation); // Draw it

                    if (totalElapsedTime - _ongoingTextureDraws[ongoingAnimation]?.DrawTime <= duration) // If the current texture draw has not yet finished
                    {
                        // Do nothing
                    }
                    else // If the current texture draw is finished
                    {
                        _ongoingTextureDraws[ongoingAnimation] = null; // Remove the current texture draw from the ongoing draws

                        if (isLastIteration) // If the current texture was the last texture in the animation
                        {
                            ongoingAnimation.HasFinished = !ongoingAnimation.Repeat; // Finish the animation, if it's not repeating
                            ongoingAnimation.IsPlaying = ongoingAnimation.Repeat; // If it's repeating, play it again

                            _ongoingTextureDraws.Remove(ongoingAnimation); // Remove the whole animation from the ongoing draws
                        }
                    }
                }
            }
        }

        private void DrawTexture(Texture2D texture, GameTime gameTime, Interpolation2D interpolation)
        {
            if (texture != null)
            {
                Vector2 position;
                float rotation;
                if (interpolation == Interpolation2D.Interpolate)
                {
                    // Interpolate position and rotation
                    position = InterpolatedDrawPosition;
                    rotation = InterpolatedDrawRotation;
                }
                else
                {
                    position = AbsolutePosition;
                    rotation = Rotation;
                }

                Vector2 origin = new(texture.Width / 2, texture.Height / 2);

                Root.SpriteBatch.Draw(texture, position, null, Color.White, rotation, origin, Scale, SpriteEffects, SortingLayer);
            }
        }

        public void Destroy(bool removeObjectFromScene = true)
        {
            Texture.Dispose();
            if (AssociatedScene != null)
            {
                object spriteReference = AssociatedScene.Sprites.FirstOrDefault(s => s == this);
                DestroyAssociatedComponents(spriteReference);
                if (removeObjectFromScene)
                    AssociatedScene.RemoveSprite(spriteReference, destroySprite: false);
                AssociatedScene = null;
            }
        }

        private static void DestroyAssociatedComponents(object spriteReference)
        {
            if (spriteReference is IPhysics2D physics2D)
            {
                physics2D.Rigidbody2D = null;
            }
            // TODO: Add more logic here if needed
        }

        internal void CollisionEnterHandler(Fixture current, Fixture other, Contact contact)
        {
            _activeCollisions.Add((current, other, contact));

            if (current.IsSensor)
                OnTriggerEnter2D(current, other, contact);
            else
                OnCollisionEnter2D(current, other, contact);
        }

        internal void CollisionExitHandler(Fixture current, Fixture other, Contact contact)
        {
            _activeCollisions.Remove((current, other, contact));

            if (current.IsSensor)
                OnTriggerExit2D(current, other, contact);
            else
                OnCollisionExit2D(current, other, contact);
        }

        public virtual void OnCollisionEnter2D(Fixture current, Fixture other, Contact contact)
        {
            // TODO: Add your collision logic to-be-inherited here
        }

        public virtual void OnCollisionStay2D(Fixture current, Fixture other, Contact contact)
        {
            // TODO: Add your collision logic to-be-inherited here
        }

        public virtual void OnCollisionExit2D(Fixture current, Fixture other, Contact contact)
        {
            // TODO: Add your collision logic to-be-inherited here
        }

        public virtual void OnTriggerEnter2D(Fixture current, Fixture other, Contact contact)
        {
            // TODO: Add your trigger logic to-be-inherited here
        }

        public virtual void OnTriggerStay2D(Fixture current, Fixture other, Contact contact)
        {
            // TODO: Add your trigger logic to-be-inherited here
        }

        public virtual void OnTriggerExit2D(Fixture current, Fixture other, Contact contact)
        {
            // TODO: Add your trigger logic to-be-inherited here
        }
    }
}