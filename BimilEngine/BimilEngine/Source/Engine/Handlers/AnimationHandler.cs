using System;
using System.Collections.Generic;
using BimilEngine.Source.Engine.Models;

namespace BimilEngine.Source.Engine.Handlers
{
    public sealed class AnimationHandler : IDisposable
    {
        /// <summary>
        /// The animations, where the key is the id of the animation.
        /// </summary>
        public Dictionary<string, Animation> Animations { get; } = new();

        public void Dispose()
        {
            foreach (Animation animation in Animations.Values)
            {
                animation.Dispose();
            }
        }
    }
}