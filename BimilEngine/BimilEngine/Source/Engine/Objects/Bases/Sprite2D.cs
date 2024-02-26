using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using BimilEngine.Source.Engine.Interfaces;
using BimilEngine.Source.Engine.Models;
using IDrawable = BimilEngine.Source.Engine.Interfaces.IDrawable;
using BimilEngine.Source.Engine.Handlers;

namespace BimilEngine.Source.Engine.Objects.Bases
{
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

        public Sprite2D(string texturePath, Vector2 position, Vector2 scale, int cameraLevel = 0, string name = "", string tag = "", Scene2D associatedScene = null)
            : base(position, scale, cameraLevel, name, tag, associatedScene)
        {
            Texture = !string.IsNullOrEmpty(texturePath)
                ? Globals.Content.Load<Texture2D>(texturePath)
                : Globals.TransparentTexture;
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

        public virtual void Draw(GameTime gameTime, float interpolationAlpha = 0f, AnimationHandler animationHandler = null)
        {
            if (Texture != null)
            {
                Vector2 origin = new(Texture.Width / 2, Texture.Height / 2);

                Globals.SpriteBatch.Draw(Texture, Position, null, Color.White, Rotation, origin, Scale, SpriteEffects, SortingLayer);
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