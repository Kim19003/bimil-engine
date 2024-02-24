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

        /// <summary>
        /// Play the animation with the given id.
        /// </summary>
        public void Play(string animationId)
        {
            if (Animations.ContainsKey(animationId))
            {
                Animations[animationId].IsPlaying = true;
            }
            else
            {
                throw new Exception($"Animation with id '{animationId}' does not exist");
            }
        }

        /// <summary>
        /// Stop the animation with the given id.
        /// </summary>
        public void Stop(string animationId)
        {
            if (Animations.ContainsKey(animationId))
            {
                Animations[animationId].IsPlaying = false;
            }
            else
            {
                throw new Exception($"Animation with id '{animationId}' does not exist");
            }
        }

        public void Dispose()
        {
            foreach (Animation animation in Animations.Values)
            {
                animation.Dispose();
            }
        }
    }
}