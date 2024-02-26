using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Graphics;

namespace BimilEngine.Source.Engine.Models
{
    public sealed class Animation : IDisposable
    {
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
        /// <remarks>If HasFinished is true, this should logically be false.</remarks>
        public bool IsPlaying { get; set; } = false;
        /// <summary>
        /// Does the animation repeat?
        /// </summary>
        public bool Repeat { get; set; } = true;
        /// <summary>
        /// Has the animation finished playing?
        /// </summary>
        /// <remarks>If Repeat is true, this should logically be null. Else, if IsPlaying is true, this should logically be false.</remarks>
        public bool? HasFinished { get; set; } = null;

        public Animation()
        {

        }

        public Animation(List<(TimeSpan Duration, Texture2D Texture)> textures, bool repeat = true)
        {
            Textures.AddRange(textures);
            Repeat = repeat;
        }

        public void Dispose()
        {
            foreach ((_, Texture2D Texture) in Textures)
            {
                Texture.Dispose();
            }
        }
    }
}