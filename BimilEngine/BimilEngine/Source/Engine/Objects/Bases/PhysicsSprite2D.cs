using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using BimilEngine.Source.Engine.Interfaces;
using BimilEngine.Source.Engine.Models;
using IDrawable = BimilEngine.Source.Engine.Interfaces.IDrawable;
using Genbox.VelcroPhysics.Dynamics;
using Genbox.VelcroPhysics.Collision.ContactSystem;
using System.Collections.Generic;
using BimilEngine.Source.Engine.Handlers;
using System;
using BimilEngine.Source.Engine.Managers;

namespace BimilEngine.Source.Engine.Objects.Bases
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
        /// Draw interpolation of the sprite.
        /// </summary>
        public Interpolation2D DrawInterpolation { get; set; } = Interpolation2D.Interpolate;
        /// <summary>
        /// Draw interpolation time of the sprite.
        /// </summary>
        public float DrawInterpolationTime { get; set; } = 0.05f;
        /// <summary>
        /// Last draw time of the sprite, mainly used for draw interpolation. You should not modify this value.
        /// </summary>
        public GameTime LastDrawTime { get; set; } = null;
        /// <summary>
        /// Last position of the transform, mainly used for position interpolation. You should not modify this value.
        /// </summary>
        public Vector2 LastPosition { get; set; } = Vector2.Zero;
        /// <summary>
        /// Last draw position of the transform, mainly used for draw interpolation. You should not modify this value.
        /// </summary>
        public Vector2 LastDrawPosition { get; set; } = Vector2.Zero;
        /// <summary>
        /// Last rotation of the transform, mainly used for rotation interpolation. You should not modify this value.
        /// </summary>
        public float LastRotation { get; set; } = 0f;

        private readonly HashSet<(Fixture, Fixture, Contact)> _activeCollisions = new();

        public PhysicsSprite2D(string texturePath, Vector2 position, Vector2 scale, Vector2 physicsScale, int cameraLevel = 0, string name = "", string tag = "",
            Scene2D associatedScene = null) : base(position, scale, cameraLevel, name, tag, associatedScene)
        {
            Texture = !string.IsNullOrEmpty(texturePath)
                ? Globals.Content.Load<Texture2D>(texturePath)
                : Globals.TransparentTexture;
            PhysicsScale = physicsScale;
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

        /// <summary>
        /// Used to store the ongoing texture draws of the animations.
        /// </summary>
        private readonly Dictionary<Animation, (Texture2D Texture, TimeSpan DrawTime)?> _ongoingTextureDraws = new();
        public virtual void Draw(GameTime gameTime, float interpolationAlpha = 0f, AnimationHandler animationHandler = null)
        {
            Animation[] ongoingAnimations = animationHandler != null
                ? animationHandler.GetOngoingAnimations()
                : Array.Empty<Animation>();

            if (!ongoingAnimations.Any())
            {
                DrawTexture(Texture, gameTime, DrawInterpolation);
            }
            else
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
                    for (int i = 0; i < ongoingAnimation.Textures.Count; i++)
                    {
                        bool isLastIteration = i == ongoingAnimation.Textures.Count - 1;

                        Texture2D texture = ongoingAnimation.Textures[i].Texture; // The current texture
                        TimeSpan duration = ongoingAnimation.Textures[i].Duration; // Duration of the current texture

                        if (!_ongoingTextureDraws.ContainsKey(ongoingAnimation)) // If the whole animation is not yet started
                            _ongoingTextureDraws.Add(ongoingAnimation, (texture, totalElapsedTime)); // Start drawing the current texture
                        else if (_ongoingTextureDraws[ongoingAnimation] == null) // If the animation's previous texture draw has finished
                            _ongoingTextureDraws[ongoingAnimation] = (texture, totalElapsedTime); // Start drawing the current texture
#warning Use class instead of (Texture2D Texture, TimeSpan DrawTime) tuple, as we can't use the reference here!
                        else if (_ongoingTextureDraws[ongoingAnimation].Value.Texture != texture) // If there is an ongoing texture draw
                            continue; // Skip drawing the current texture

                        DrawTexture(texture, gameTime, DrawInterpolation); // Draw it

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
        }

        private void DrawTexture(Texture2D texture, GameTime gameTime, Interpolation2D interpolation)
        {
            if (texture != null)
            {
                Vector2 position;
                float rotation;
                if (interpolation == Interpolation2D.Interpolate)
                {
                    float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
                    float f = deltaTime / DrawInterpolationTime;
                    float interpolationAlpha = MathHelper.Clamp(deltaTime / DrawInterpolationTime, 0.0f, 1.0f);

                    // Interpolate position and rotation
                    position = Vector2.Lerp(LastDrawPosition, Position, interpolationAlpha);
                    rotation = MathHelper.Lerp(LastRotation, Rotation, interpolationAlpha);
                }
                else
                {
                    position = Position;
                    rotation = Rotation;
                }

                Vector2 origin = new(texture.Width / 2, texture.Height / 2);

                Globals.SpriteBatch.Draw(texture, position, null, Color.White, rotation, origin, Scale, SpriteEffects, SortingLayer);

                LastDrawPosition = position;
                LastDrawTime = gameTime;
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