using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Bimil.Engine.Models;

namespace Bimil.Engine.Other.Extensions
{
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
}