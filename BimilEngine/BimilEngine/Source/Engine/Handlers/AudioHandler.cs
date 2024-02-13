using System.Collections.Generic;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework;
using System.Linq;
using System;

namespace BimilEngine.Source.Engine.Handlers
{
    public sealed class AudioHandler : IDisposable
    {
        /// <summary>
        /// The songs, where the key is the id of the song.
        /// </summary>
        public Dictionary<string, Song> Songs { get; } = new();
        /// <summary>
        /// The sound effects, where the key is the id of the sound effect.
        /// </summary>
        public Dictionary<string, SoundEffect> SoundEffects { get; } = new();

        private readonly AudioListener _audioListener = new();
        private readonly AudioEmitter _audioEmitter = new();
        
        private readonly HashSet<SoundEffectInstance> _createdSoundEffectInstances = new();

        public void Play3DSoundEffect(string soundEffectId, Vector2 perceiverPosition, Vector2 sourcePosition)
        {
            Play3DSoundEffect(soundEffectId, new Vector3(perceiverPosition, 0), new Vector3(sourcePosition, 0));
        }

        public void Play3DSoundEffect(string soundEffectId, Vector3 perceiverPosition, Vector3 sourcePosition)
        {
            DisposeAllFinishedSoundEffectInstances();

            if (SoundEffects.ContainsKey(soundEffectId))
            {
                SoundEffectInstance soundEffectInstance = SoundEffects[soundEffectId].CreateInstance();
                _audioListener.Position = perceiverPosition;
                _audioEmitter.Position = sourcePosition;
                soundEffectInstance.Apply3D(_audioListener, _audioEmitter);
                soundEffectInstance.Play();

                _createdSoundEffectInstances.Add(soundEffectInstance);
                _audioListener.Position = Vector3.Zero;
                _audioEmitter.Position = Vector3.Zero;
            }
            else
            {
                throw new Exception($"Sound effect with id '{soundEffectId}' does not exist");
            }
        }

        private void DisposeAllFinishedSoundEffectInstances()
        {
            IEnumerable<SoundEffectInstance> finishedSoundEffectInstances = _createdSoundEffectInstances.Where(x => x.State == SoundState.Stopped);
            
            foreach (SoundEffectInstance finishedSoundEffectInstance in finishedSoundEffectInstances)
            {
                finishedSoundEffectInstance.Dispose();
                _createdSoundEffectInstances.Remove(finishedSoundEffectInstance);
            }
        }

        public void Dispose()
        {
            foreach (SoundEffectInstance soundEffectInstance in _createdSoundEffectInstances)
            {
                soundEffectInstance.Dispose();
            }
        }
    }
}