using Microsoft.Xna.Framework;

namespace Bimil.Engine
{
    public static class Settings
    {
        public static float PhysicsHertz { get; set; } = 120f;
        public static int DefaultScreenWidth { get; set; } = 1280;
        public static int DefaultScreenHeight { get; set; } = 720;
        public static int DefaultMaxFPS { get; set; } = 999;
        public static Vector2 PhysicsWorldGravity { get; set; } = new(0, 200f);

        public static class Physics
        {
            public static int PositionIterations { get; set; } = 6;
            public static int VelocityIterations { get; set; } = 16;
            public static float MaxTranslation { get; set; } = 200f;
        }
    }
}