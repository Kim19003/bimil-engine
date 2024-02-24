using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Graphics;

namespace BimilEngine.Source.Engine.Models
{
    public sealed class Animation : IDisposable
    {
        public string Id { get; set; }
        public List<(TimeSpan Duration, Texture2D Texture)> Textures { get; } = new();
        public TimeSpan Duration => Textures.Any()
            ? Textures.Select(t => t.Duration).Aggregate((a, b) => a + b)
            : TimeSpan.Zero;

        public void Dispose()
        {
            foreach ((_, Texture2D Texture) in Textures)
            {
                Texture.Dispose();
            }
        }
    }
}