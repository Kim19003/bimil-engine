using Microsoft.Xna.Framework;
using Bimil.Engine.Objects.Bases;
using Bimil.Engine.Handlers;

namespace Bimil.Game.Sprites
{
    public class Object : PhysicsSprite2D
    {
        public Object(string textureName, Vector2 position, Scene2D associatedScene = null)
            : base(textureName, associatedScene)
        {
            Position = position;
        }

        public override void Update(GameTime gameTime)
        {
            // ---------
            base.Update(gameTime);
        }

        public override void FixedUpdate(GameTime gameTime, GameTime fixedGameTime)
        {

            // ---------
            base.FixedUpdate(gameTime, fixedGameTime);
        }

        public override void Draw(GameTime gameTime, AnimationHandler animationHandler = null)
        {

            // ---------
            base.Draw(gameTime, animationHandler);
        }
    }
}