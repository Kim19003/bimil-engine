using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using BimilEngine.Source.Engine.Interfaces;
using BimilEngine.Source.Engine.Models;
using IDrawable = BimilEngine.Source.Engine.Interfaces.IDrawable;
using Genbox.VelcroPhysics.Dynamics;
using Genbox.VelcroPhysics.Collision.ContactSystem;
using System.Collections.Generic;

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
        /// Last position of the transform, used for interpolation. You should not modify this value.
        /// </summary>
        public Vector2 LastPosition { get; set; } = Vector2.Zero;
        /// <summary>
        /// Last rotation of the transform, used for interpolation. You should not modify this value.
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

        public virtual void Draw(GameTime gameTime, float interpolationAlpha = 0f)
        {
            if (Texture != null)
            {
                Vector2 position;
                float rotation;
                if (Rigidbody2D.Interpolation == RigidbodyInterpolation2D.Interpolate)
                {
                    // Interpolate position and rotation
                    position = Vector2.Lerp(LastPosition, Position, interpolationAlpha);
                    rotation = MathHelper.Lerp(LastRotation, Rotation, interpolationAlpha);
                }
                else
                {
                    position = Position;
                    rotation = Rotation;
                }

                Vector2 origin = new(Texture.Width / 2, Texture.Height / 2);

                Globals.SpriteBatch.Draw(Texture, position, null, Color.White, rotation, origin, Scale, SpriteEffects, SortingLayer);
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