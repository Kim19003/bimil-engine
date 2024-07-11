using System;
using System.Collections.Generic;
using System.Linq;
using Bimil.Engine.Models;
using Bimil.Engine.Other;

namespace Bimil.Engine.Handlers
{
    public sealed class AnimationHandler : IDisposable
    {
        /// <summary>
        /// The animations, where the key is the id of the animation.
        /// </summary>
        public Dictionary<string, Animation> Animations { get; } = new();

        /// <summary>
        /// Add an animation to the handler.
        /// </summary>
        public void AddAnimation(string id, Animation animation)
        {
            if (Animations.ContainsKey(id))
            {
                throw new Exception($"Animation with id '{id}' already exists");
            }

            Animations.Add(id, animation);
        }

        /// <summary>
        /// Add animations to the handler.
        /// </summary>
        public void AddAnimations((string Id, Animation Animation)[] animations)
        {
            foreach ((string Id, Animation Animation) in animations)
            {
                AddAnimation(Id, Animation);
            }
        }

        /// <summary>
        /// Remove an animation from the handler.
        /// </summary>
        public void RemoveAnimation(string id)
        {
            if (Animations.ContainsKey(id))
            {
                Animations[id].Dispose();
                Animations.Remove(id);
            }
            else
            {
                throw new Exception($"Animation with id '{id}' does not exist");
            }
        }

        /// <summary>
        /// Remove animations from the handler.
        /// </summary>
        public void RemoveAnimations(string[] ids)
        {
            foreach (string id in ids)
            {
                RemoveAnimation(id);
            }
        }

        /// <summary>
        /// Play the animation with the given id. Use this to ensure that only one animation is playing at a time.
        /// </summary>
        public void PlayAnimation(string animationId)
        {
            if (Animations.ContainsKey(animationId))
            {
                Animations[animationId].IsPlaying = true;

                foreach (var animation in Animations)
                {
                    if (animation.Key != animationId)
                    {
                        animation.Value.IsPlaying = false;
                    }
                }
            }
            else
            {
                throw new Exception($"Animation with id '{animationId}' does not exist");
            }
        }

        /// <summary>
        /// Stop the animation with the given id.
        /// </summary>
        public void StopAnimation(string animationId)
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

        /// <summary>
        /// Get all the ongoing animations.
        /// </summary>
        public Animation[] GetOngoingAnimations()
        {
            return Animations.Values.Where(animation => animation.IsPlaying && BooleanExtensions.IsNullOrFalse(animation.HasFinished)).ToArray();
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