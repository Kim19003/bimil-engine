using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Bimil.Engine.Models;
using Bimil.Engine.Objects;
using Color = Microsoft.Xna.Framework.Color;
using Rectangle = Microsoft.Xna.Framework.Rectangle;
using Bimil.Engine.Objects.Bases;
using Genbox.VelcroPhysics.Dynamics;
using Bimil.Engine.Models.DrawShapes;

namespace Bimil.Engine.Other
{
    public static class QuickDirection2DExtensions
    {
        /// <summary>
        /// If the quick direction is up or down.
        /// </summary>
        public static bool IsUpOrDown(this QuickDirection2D quickDirection2D)
        {
            return quickDirection2D == QuickDirection2D.Up || quickDirection2D == QuickDirection2D.Down;
        }

        /// <summary>
        /// If the quick direction is up-right or down-left.
        /// </summary>
        public static bool IsUpRightOrDownLeft(this QuickDirection2D quickDirection2D)
        {
            return quickDirection2D == QuickDirection2D.UpRight || quickDirection2D == QuickDirection2D.DownLeft;
        }

        /// <summary>
        /// If the quick direction is left or right.
        /// </summary>
        public static bool IsLeftOrRight(this QuickDirection2D quickDirection2D)
        {
            return quickDirection2D == QuickDirection2D.Left || quickDirection2D == QuickDirection2D.Right;
        }

        /// <summary>
        /// If the quick direction is up-left or down-right.
        /// </summary>
        public static bool IsUpLeftOrDownRight(this QuickDirection2D quickDirection2D)
        {
            return quickDirection2D == QuickDirection2D.UpLeft || quickDirection2D == QuickDirection2D.DownRight;
        }

        /// <summary>
        /// If the quick direction is anything but none.
        /// </summary>
        public static bool IsAny(this QuickDirection2D quickDirection2D)
        {
            return quickDirection2D != QuickDirection2D.None;
        }
    }

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

            DrawPolygon(spriteBatch, new(vertices, rectangleDrawShape.Color, rectangleDrawShape.LineThickness, rectangleDrawShape.LayerDepth));
        }

        public static void DrawPolygon(this SpriteBatch spriteBatch, PolygonDrawShape polygonDrawShape)
        {
            for (int i = 0; i < polygonDrawShape.Vertices.Length; i++)
            {
                Vector2 start = polygonDrawShape.Vertices[i];
                Vector2 end = polygonDrawShape.Vertices[(i + 1) % polygonDrawShape.Vertices.Length];

                DrawLine(spriteBatch, new(start, end, polygonDrawShape.Color, polygonDrawShape.LineThickness, polygonDrawShape.LayerDepth));
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

                DrawLine(spriteBatch, new(position + start, position + end, circleDrawShape.Color, circleDrawShape.LineThickness, circleDrawShape.LayerDepth));

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

    public static class KeyboardStateExtensions
    {
        private static readonly Dictionary<Keys, bool> previousKeyStates = new();

        public static bool IsKeyPressed(this KeyboardState currentKeyboardState, Keys key)
        {
            // Check if the key is in the dictionary
            if (!previousKeyStates.ContainsKey(key))
            {
                // If not, add it with an initial value of false
                previousKeyStates.Add(key, false);
            }

            // Check if the key is pressed in the current state and was not pressed in the previous state
            bool isKeyPressedOnce = currentKeyboardState.IsKeyDown(key) && !previousKeyStates[key];

            // Update the previous key state for the next frame
            previousKeyStates[key] = currentKeyboardState.IsKeyDown(key);

            return isKeyPressedOnce;
        }
    }

    public static class ArrayExtensions
    {
        public static Vector2 GetFurthest(this Vector2[] positions, Perspective perspective)
        {
            return perspective switch
            {
                Perspective.LeftToRight => positions.OrderByDescending(p => p.X).First(),
                Perspective.RightToLeft => positions.OrderBy(p => p.X).First(),
                Perspective.TopToBottom => positions.OrderByDescending(p => p.Y).First(),
                Perspective.BottomToTop => positions.OrderBy(p => p.Y).First(),
                _ => throw new ArgumentOutOfRangeException(nameof(perspective), perspective, null),
            };
        }
    }

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

    public static class DictionaryExtensions
    {
        /// <summary>
        /// Adds the log to the shown screen logs sequence. Sets a order number for the log to follow the sequence logic.
        /// </summary>
        public static void AddToSequence(this Dictionary<int, Log> shownScreenLogs, Log log)
        {
            int mininumOrderNumber = shownScreenLogs.Any() ? shownScreenLogs.Keys.Max() : 0;

            shownScreenLogs.Add(++mininumOrderNumber, log);
        }

        /// <summary>
        /// Rearranges the shown screen logs sequence. Sets new order number and position for every log to follow the sequence logic.
        /// </summary>
        public static void RearrangeSequence(this Dictionary<int, Log> shownScreenLogs, Vector2 logScreenStartPosition, int verticalSpacing)
        {
            Dictionary<int, Log> newShownScreenLogs = new();

            int orderNumber = 1;
            Vector2 previousLogPosition = Vector2.Zero;
            foreach (var shownScreenLog in shownScreenLogs.OrderBy(x => x.Key))
            {
                shownScreenLog.Value.Position = orderNumber == 1
                    ? logScreenStartPosition
                    : previousLogPosition + new Vector2(0, verticalSpacing);
                previousLogPosition = shownScreenLog.Value.Position;

                newShownScreenLogs.Add(orderNumber, shownScreenLog.Value);

                orderNumber++;
            }

            shownScreenLogs.Clear();
            foreach (var newShownScreenLog in newShownScreenLogs)
            {
                shownScreenLogs.Add(newShownScreenLog.Key, newShownScreenLog.Value);
            }
        }
    }

    public static class Vector2Extensions
    {
        /// <summary>
        /// Multiplies the following vector by the parent vector.
        /// </summary>
        /// <returns>The following vector multiplied by the parent vector.</returns>
        public static Vector2 MultiplyFollowing(this Vector2 multiplier, Vector2 following)
        {
            return new(following.X * multiplier.X, following.Y * multiplier.Y);
        }

        /// <summary>
        /// Divides the following vector by the parent vector.
        /// </summary>
        /// <returns>The following vector divided by the parent vector.</returns>
        public static Vector2 DivideFollowing(this Vector2 divider, Vector2 following)
        {
            Vector2 logicalScale = new(following.X / divider.X, following.Y / divider.Y);

            return logicalScale;
        }

        /// <summary>
        /// Subtracts the specified vector from the current vector and updates the current vector.
        /// </summary>
        /// <param name="vector">The vector to be updated.</param>
        /// <param name="value">The vector to subtract from the current vector.</param>
        public static void Subtract(this Vector2 vector, Vector2 value)
        {
            vector.X -= value.X;
            vector.Y -= value.Y;
        }

        /// <summary>
        /// Adds the specified vector to the current vector and updates the current vector.
        /// </summary>
        /// <param name="vector">The vector to be updated.</param>
        /// <param name="value">The vector to add to the current vector.</param>
        public static void Add(this Vector2 vector, Vector2 value)
        {
            vector.X += value.X;
            vector.Y += value.Y;
        }

        /// <summary>
        /// Multiplies the current vector by the specified vector and updates the current vector.
        /// </summary>
        /// <param name="vector">The vector to be updated.</param>
        /// <param name="value">The vector to multiply with the current vector.</param>
        public static void Scale(this Vector2 vector, Vector2 value)
        {
            vector.X *= value.X;
            vector.Y *= value.Y;
        }

        /// <summary>
        /// Divides the current vector by the specified vector and updates the current vector.
        /// </summary>
        /// <param name="vector">The vector to be updated.</param>
        /// <param name="value">The vector to divide the current vector by.</param>
        /// <exception cref="DivideByZeroException">Thrown when any component of the divisor vector is zero.</exception>
        public static void DivideInPlace(this Vector2 vector, Vector2 value)
        {
            if (value.X == 0 || value.Y == 0)
            {
                throw new DivideByZeroException("Cannot divide by a vector with zero components.");
            }
            vector.X /= value.X;
            vector.Y /= value.Y;
        }
    }

    public static class BodyExtensions
    {
        /// <summary>
        /// Tries to get the parent of the body.
        /// </summary>
        /// <returns>If found, the parent of the body. Otherwise, null.</returns>
        public static PhysicsSprite2D GetParent(this Body body)
        {
            return (PhysicsSprite2D)body?.UserData;
        }
    }

    public static class FixtureExtensions
    {
        /// <summary>
        /// Tries to get the parent of the fixture's body.
        /// </summary>
        /// <returns>If found, the parent of the fixture's body. Otherwise, null.</returns>
        public static PhysicsSprite2D GetParent(this Fixture fixture)
        {
            return (PhysicsSprite2D)fixture?.Body?.UserData;
        }
    }

    public static class BooleanExtensions
    {
        public static bool IsNullOrTrue(bool? value)
        {
            return value == null || value == true;
        }

        public static bool IsNullOrFalse(bool? value)
        {
            return value == null || value == false;
        }
    }
}