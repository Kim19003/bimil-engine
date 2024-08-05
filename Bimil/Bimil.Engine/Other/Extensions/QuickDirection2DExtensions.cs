using Bimil.Engine.Models;

namespace Bimil.Engine.Other.Extensions
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
}