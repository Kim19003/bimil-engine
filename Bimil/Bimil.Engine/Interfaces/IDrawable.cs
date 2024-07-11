using Bimil.Engine.Handlers;
using Microsoft.Xna.Framework;

namespace Bimil.Engine.Interfaces
{
    public interface IDrawable
    {
        void Draw(GameTime gameTime, AnimationHandler animationHandler = null);
    }
}