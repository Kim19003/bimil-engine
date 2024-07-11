using Microsoft.Xna.Framework;

namespace Bimil.Engine.Interfaces
{
    public interface IUpdatable
    {
        void Update(GameTime gameTime);
        void FixedUpdate(GameTime gameTime, GameTime fixedGameTime);
    }
}