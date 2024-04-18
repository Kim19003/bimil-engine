using System;
using Microsoft.Xna.Framework.Graphics;

namespace BimilEngine.Source.Engine.Models
{
    public class DuratedTexture
    {
        public TimeSpan Duration { get; set; }
        public Texture2D Texture { get; set; }

        public DuratedTexture(TimeSpan duration, Texture2D texture)
        {
            Duration = duration;
            Texture = texture;
        }
    }
}