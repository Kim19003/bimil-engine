using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Bimil.Engine.Models.DrawShapes;

namespace Bimil.Engine.Other.Extensions
{
    public static class SpriteBatchExtensions
    {
        public static void DrawRectangle(this SpriteBatch spriteBatch, RectangleDrawShape rectangleDrawShape)
        {
            Vector2[] vertices = new Vector2[]
            {
                new(rectangleDrawShape.Body.X, rectangleDrawShape.Body.Y),
                new(rectangleDrawShape.Body.X + rectangleDrawShape.Body.Width, rectangleDrawShape.Body.Y),
                new(rectangleDrawShape.Body.X + rectangleDrawShape.Body.Width, rectangleDrawShape.Body.Y + rectangleDrawShape.Body.Height),
                new(rectangleDrawShape.Body.X, rectangleDrawShape.Body.Y + rectangleDrawShape.Body.Height)
            };

            spriteBatch.DrawPolygon(new(vertices, rectangleDrawShape.Color, rectangleDrawShape.LineThickness, rectangleDrawShape.LayerDepth));
        }

        public static void DrawPolygon(this SpriteBatch spriteBatch, PolygonDrawShape polygonDrawShape)
        {
            for (int i = 0; i < polygonDrawShape.Vertices.Length; i++)
            {
                Vector2 start = polygonDrawShape.Vertices[i];
                Vector2 end = polygonDrawShape.Vertices[(i + 1) % polygonDrawShape.Vertices.Length];

                spriteBatch.DrawLine(new(start, end, polygonDrawShape.Color, polygonDrawShape.LineThickness, polygonDrawShape.LayerDepth));
            }
        }

        public static void DrawCircle(this SpriteBatch spriteBatch, CircleDrawShape circleDrawShape)
        {
            float angle = 0f;
            float angleStep = 0.1f;

            Vector2 position = Helpers.ConvertToVector2(circleDrawShape.Body.Location);
            int radius = circleDrawShape.Body.Radius;

            while (angle < Math.PI * 2)
            {
                Vector2 start = new((float)Math.Cos(angle) * radius, (float)Math.Sin(angle) * radius);
                Vector2 end = new((float)Math.Cos(angle + angleStep) * radius, (float)Math.Sin(angle + angleStep) * radius);

                spriteBatch.DrawLine(new(position + start, position + end, circleDrawShape.Color, circleDrawShape.LineThickness, circleDrawShape.LayerDepth));

                angle += angleStep;
            }
        }

        public static void DrawLine(this SpriteBatch spriteBatch, LineDrawShape lineDrawShape)
        {
            Vector2 edge = lineDrawShape.End - lineDrawShape.Start;
            float angle = (float)Math.Atan2(edge.Y, edge.X);

            spriteBatch.Draw(Root.PixelTexture, lineDrawShape.Start, null, lineDrawShape.Color, angle, Vector2.Zero, new Vector2(edge.Length(),
                lineDrawShape.LineThickness), SpriteEffects.None, lineDrawShape.LayerDepth);
        }
    }
}