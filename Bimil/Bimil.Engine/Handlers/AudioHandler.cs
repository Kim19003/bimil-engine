using System.Collections.Generic;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework;
using System.Linq;
using System;

namespace Bimil.Engine.Handlers
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

        /// <summary>
        /// The audio listener.
        /// </summary>
        public AudioListener AudioListener => _audioListener;
        private readonly AudioListener _audioListener = new();
        /// <summary>
        /// The audio emitter.
        /// </summary>
        public AudioEmitter AudioEmitter => _audioEmitter;
        private readonly AudioEmitter _audioEmitter = new();
        
        /// <summary>
        /// The created sound effect instances, where the key is the sound effect.
        /// </summary>
        public IReadOnlyDictionary<SoundEffect, IReadOnlyCollection<SoundEffectInstance>> SoundEffectInstances => _soundEffectInstances
            .ToDictionary(x => x.Key, x => x.Value as IReadOnlyCollection<SoundEffectInstance>);
        private readonly Dictionary<SoundEffect, HashSet<SoundEffectInstance>> _soundEffectInstances = new();

        public void PlaySong(string songId, bool isRepeating = true)
        {
            if (Songs.ContainsKey(songId))
            {
                MediaPlayer.IsRepeating = isRepeating;
                MediaPlayer.Play(Songs[songId]);
            }
            else
            {
                throw new Exception($"Song with id '{songId}' does not exist");
            }
        }

        public void PlaySoundEffect(string soundEffectId, Vector2 perceiverPosition, Vector2 sourcePosition, int? distanceHotspot = 1000)
        {
            Play3DSoundEffect(soundEffectId, new Vector3(perceiverPosition, 0), new Vector3(sourcePosition, 0), distanceHotspot);
        }

        private void Play3DSoundEffect(string soundEffectId, Vector3 perceiverPosition, Vector3 sourcePosition, int? distanceHotspot)
        {
            DisposeStoppedSoundEffectInstances();

            if (SoundEffects.ContainsKey(soundEffectId))
            {
                SoundEffect soundEffect = SoundEffects[soundEffectId];
                SoundEffectInstance soundEffectInstance = soundEffect.CreateInstance();

                _audioListener.Position = perceiverPosition;
                _audioEmitter.Position = sourcePosition;
                soundEffectInstance.Apply3D(_audioListener, _audioEmitter);

                if (distanceHotspot != null)
                {
                    float distance = Vector3.Distance(perceiverPosition, sourcePosition);
                    soundEffectInstance.Volume = 1 - MathHelper.Clamp(distance / (int)distanceHotspot, 0, 1);
                }

                soundEffectInstance.Play();

                if (!_soundEffectInstances.ContainsKey(soundEffect))
                    _soundEffectInstances.Add(soundEffect, new() { soundEffectInstance });
                else
                    _soundEffectInstances[soundEffect].Add(soundEffectInstance);
                    
                _audioListener.Position = Vector3.Zero;
                _audioEmitter.Position = Vector3.Zero;
            }
            else
            {
                throw new Exception($"Sound effect with id '{soundEffectId}' does not exist");
            }
        }

        private void DisposeStoppedSoundEffectInstances()
        {
            foreach (var soundEffectInstances in _soundEffectInstances)
            {
                foreach (SoundEffectInstance soundEffectInstance in soundEffectInstances.Value)
                {
                    if (soundEffectInstance.State == SoundState.Stopped)
                    {
                        soundEffectInstance.Dispose();
                        _soundEffectInstances[soundEffectInstances.Key].Remove(soundEffectInstance);
                    }
                }
            }
        }

        public void Dispose()
        {
            foreach (Song song in Songs.Values)
            {
                song.Dispose();
            }

            foreach (var soundEffectInstances in _soundEffectInstances.Values)
            {
                foreach (SoundEffectInstance soundEffectInstance in soundEffectInstances)
                {
                    soundEffectInstance.Dispose();
                }
            }

            foreach (SoundEffect soundEffect in SoundEffects.Values)
            {
                soundEffect.Dispose();
            }
        }
    }
}