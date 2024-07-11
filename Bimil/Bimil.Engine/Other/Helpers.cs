using System.Drawing;
using Vector2 = Microsoft.Xna.Framework.Vector2;
using Point = Microsoft.Xna.Framework.Point;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using Rectangle = Microsoft.Xna.Framework.Rectangle;

namespace Bimil.Engine.Other
{
    public static class Helpers
    {
        public static Point ConvertToPoint(Vector2 vector2)
        {
            return new((int)vector2.X, (int)vector2.Y);
        }

        public static PointF ConvertToPointF(Vector2 vector2)
        {
            return new(vector2.X, vector2.Y);
        }

        public static Vector2 ConvertToVector2(Point point)
        {
            return new(point.X, point.Y);
        }

        public static Vector2 ConvertToVector2(PointF pointF)
        {
            return new(pointF.X, pointF.Y);
        }

        public static Rectangle GetWorldPointBounds(Viewport viewport, Matrix transformMatrix)
        {
            Matrix inverseTransform = Matrix.Invert(transformMatrix);

            Vector2 topLeft = Vector2.Transform(new Vector2(-viewport.X, -viewport.Y), inverseTransform);
            Vector2 topRight = Vector2.Transform(new Vector2(-viewport.X + viewport.Width, -viewport.Y), inverseTransform);
            Vector2 bottomLeft = Vector2.Transform(new Vector2(-viewport.X, -viewport.Y + viewport.Height), inverseTransform);
            Vector2 bottomRight = Vector2.Transform(new Vector2(-viewport.X + viewport.Width, -viewport.Y + viewport.Height), inverseTransform);

            float minX = Math.Min(topLeft.X, Math.Min(topRight.X, Math.Min(bottomLeft.X, bottomRight.X)));
            float minY = Math.Min(topLeft.Y, Math.Min(topRight.Y, Math.Min(bottomLeft.Y, bottomRight.Y)));
            float maxX = Math.Max(topLeft.X, Math.Max(topRight.X, Math.Max(bottomLeft.X, bottomRight.X)));
            float maxY = Math.Max(topLeft.Y, Math.Max(topRight.Y, Math.Max(bottomLeft.Y, bottomRight.Y)));

            return new Rectangle((int)minX, (int)minY, (int)(maxX - minX), (int)(maxY - minY));
        }
    }
}