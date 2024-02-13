using Microsoft.Xna.Framework;

namespace BimilEngine.Source.Engine.Interfaces
{
    public interface IUpdatable
    {
        void Update(GameTime gameTime);
        void FixedUpdate(GameTime gameTime, GameTime fixedGameTime);
    }
}