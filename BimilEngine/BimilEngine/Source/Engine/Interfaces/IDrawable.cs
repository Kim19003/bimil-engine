using BimilEngine.Source.Engine.Handlers;
using Microsoft.Xna.Framework;

namespace BimilEngine.Source.Engine.Interfaces
{
    public interface IDrawable
    {
        void Draw(GameTime gameTime, float interpolationAlpha, AnimationHandler animationHandler = null);
    }
}