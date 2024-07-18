using Microsoft.Xna.Framework;

namespace Bimil.Engine
{
    public class Settings
    {
        public float PhysicsHertz { get; set; } = 120f;
        public int DefaultScreenWidth { get; set; } = 1280;
        public int DefaultScreenHeight { get; set; } = 720;
        public int DefaultMaxFPS { get; set; } = 999;
        public Vector2 PhysicsWorldGravity { get; set; } = new(0, 200f);

        public PhysicsSettings Physics { get; } = new();
    }

    public class PhysicsSettings
    {
        public int PositionIterations { get; set; } = 6;
        public int VelocityIterations { get; set; } = 16;
        public float MaxTranslation { get; set; } = 200f;
    }
}