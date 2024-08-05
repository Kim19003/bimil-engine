using Microsoft.Xna.Framework;
using Bimil.Engine.Objects;
using Rectangle = Microsoft.Xna.Framework.Rectangle;

namespace Bimil.Engine.Other.Extensions
{
    public static class Camera2DExtensions
    {
        public static Vector2 ConvertToWorldPoint(this Camera2D camera, Vector2 screenPoint)
        {
            Rectangle worldPointBounds = Helpers.GetWorldPointBounds(camera.Viewport, camera.Matrix);
            return new(screenPoint.X + worldPointBounds.X, screenPoint.Y + worldPointBounds.Y);
        }

        public static Vector2 ConvertToScreenPoint(this Camera2D camera, Vector2 worldPoint)
        {
            Rectangle worldPointBounds = Helpers.GetWorldPointBounds(camera.Viewport, camera.Matrix);
            return new(worldPoint.X - worldPointBounds.X, worldPoint.Y - worldPointBounds.Y);
        }
    }
}