using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Graphics;

namespace BimilEngine.Source.Engine.Models
{
    public sealed class Animation : IDisposable
    {
        /// <summary>
        /// The durated textures (textures with durations) in the animation.
        /// </summary>
        public HashSet<DuratedTexture> DuratedTextures { get; } = new();
        /// <summary>
        /// The duration of the animation.
        /// </summary>
        public TimeSpan Duration => DuratedTextures.Any()
            ? DuratedTextures.Select(t => t.Duration).Aggregate((a, b) => a + b)
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

        public Animation(DuratedTexture duratedTexture, bool repeat = true)
        {
            DuratedTextures.Add(duratedTexture);
            Repeat = repeat;
        }

        public Animation(HashSet<DuratedTexture> duratedTextures, bool repeat = true)
        {
            foreach (DuratedTexture duratedTexture in duratedTextures)
            {
                DuratedTextures.Add(duratedTexture);
            }
            Repeat = repeat;
        }

        public void Dispose()
        {
            foreach (DuratedTexture duratedTexture in DuratedTextures)
            {
                duratedTexture.Texture.Dispose();
            }
        }
    }
}