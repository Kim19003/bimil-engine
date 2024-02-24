using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Graphics;

namespace BimilEngine.Source.Engine.Models
{
    public sealed class Animation : IDisposable
    {
        /// <summary>
        /// The id of the animation.
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// The textures and their durations in the animation.
        /// </summary>
        public List<(TimeSpan Duration, Texture2D Texture)> Textures { get; } = new();
        /// <summary>
        /// The duration of the animation.
        /// </summary>
        public TimeSpan Duration => Textures.Any()
            ? Textures.Select(t => t.Duration).Aggregate((a, b) => a + b)
            : TimeSpan.Zero;
        /// <summary>
        /// Is the animation playing?
        /// </summary>
        public bool IsPlaying { get; set; } = false;
        /// <summary>
        /// Does the animation repeat?
        /// </summary>
        public bool Repeat { get; set; } = true;
        /// <summary>
        /// Has the animation finished playing? Use this when the animation is not repeating.
        /// </summary>
        public bool? HasFinished { get; set; } = null;

        public void Dispose()
        {
            foreach ((_, Texture2D Texture) in Textures)
            {
                Texture.Dispose();
            }
        }
    }
}