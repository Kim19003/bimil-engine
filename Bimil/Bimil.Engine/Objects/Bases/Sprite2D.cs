using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Bimil.Engine.Interfaces;
using Bimil.Engine.Models;
using IDrawable = Bimil.Engine.Interfaces.IDrawable;
using Bimil.Engine.Handlers;
using System;

namespace Bimil.Engine.Objects.Bases
{
    [Obsolete("This class is obsolete and will be removed in the future. Use PhysicsSprite2D instead.")]
    public abstract class Sprite2D : Transform2D, IStartable, IDrawable, IUpdatable, IDestroyable
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
        /// Absolute position of the transform.
        /// </summary>
        public Vector2 AbsolutePosition
        {
            get
            {
                return Position * new Vector2(Texture.Width, Texture.Height);
            }
        }

        public Sprite2D(string textureName, Scene2D associatedScene = null)
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
            // TODO: Add your fixed update logic to-be-inherited here
        }

        public virtual void Draw(GameTime gameTime, AnimationHandler animationHandler = null)
        {
            if (Texture != null)
            {
                Vector2 origin = new(Texture.Width / 2, Texture.Height / 2);

                Root.SpriteBatch.Draw(Texture, AbsolutePosition, null, Color.White, Rotation, origin, Scale, SpriteEffects, SortingLayer);
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
            // TODO: Add more logic here if needed
        }

        public virtual void OnCollisionEnter2D(Collision2D collision)
        {
            // TODO: Add your collision logic to-be-inherited here
        }

        public virtual void OnCollisionStay2D(Collision2D collision)
        {
            // TODO: Add your collision logic to-be-inherited here
        }

        public virtual void OnCollisionExit2D(Collision2D collision)
        {
            // TODO: Add your collision logic to-be-inherited here
        }

        public virtual void OnTriggerEnter2D(Collision2D collision)
        {
            // TODO: Add your trigger logic to-be-inherited here
        }

        public virtual void OnTriggerStay2D(Collision2D collision)
        {
            // TODO: Add your trigger logic to-be-inherited here
        }

        public virtual void OnTriggerExit2D(Collision2D collision)
        {
            // TODO: Add your trigger logic to-be-inherited here
        }
    }
}