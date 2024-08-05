using System;
using Microsoft.Xna.Framework;

namespace Bimil.Engine.Other.Extensions
{
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
}