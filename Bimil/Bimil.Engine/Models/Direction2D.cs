using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace Bimil.Engine.Models
{
    public static class Direction2D
    {
        /// <summary>
        /// Get the left direction.
        /// </summary>
        /// <returns>New Vector2(-1, 0).</returns>
        public static Vector2 Left => new(-1, 0);

        /// <summary>
        /// Get the up-left direction.
        /// </summary>
        /// <returns>New Vector2(-1, -1).</returns>
        public static Vector2 UpLeft => new(-1, -1);

        /// <summary>
        /// Get the down-left direction.
        /// </summary>
        /// <returns>New Vector2(-1, 1).</returns>
        public static Vector2 DownLeft => new(-1, 1);

        /// <summary>
        /// Get the right direction.
        /// </summary>
        /// <returns>New Vector2(1, 0).</returns>
        public static Vector2 Right => new(1, 0);

        /// <summary>
        /// Get the up-right direction.
        /// </summary>
        /// <returns>New Vector2(1, -1).</returns>
        public static Vector2 UpRight => new(1, -1);

        /// <summary>
        /// Get the down-right direction.
        /// </summary>
        /// <returns>New Vector2(1, 1).</returns>
        public static Vector2 DownRight => new(1, 1);

        /// <summary>
        /// Get the up direction.
        /// </summary>
        /// <returns>New Vector2(0, -1).</returns>
        public static Vector2 Up => new(0, -1);

        /// <summary>
        /// Get the down direction.
        /// </summary>
        /// <returns>New Vector2(0, 1).</returns>
        public static Vector2 Down => new(0, 1);

        /// <summary>
        /// Get the none direction.
        /// </summary>
        /// <returns>New Vector2(0, 0).</returns>
        public static Vector2 None => new(0, 0);
    }
}