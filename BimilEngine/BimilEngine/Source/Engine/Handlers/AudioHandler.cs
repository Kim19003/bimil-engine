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
        
        private readonly Dictionary<SoundEffect, SoundEffectInstance> _soundEffectInstances = new();

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

        public void PlaySoundEffect(string soundEffectId, Vector2 perceiverPosition, Vector2 sourcePosition)
        {
            Play3DSoundEffect(soundEffectId, new Vector3(perceiverPosition, 0), new Vector3(sourcePosition, 0));
        }

        private void Play3DSoundEffect(string soundEffectId, Vector3 perceiverPosition, Vector3 sourcePosition)
        {
            if (SoundEffects.ContainsKey(soundEffectId))
            {
                SoundEffect soundEffect = SoundEffects[soundEffectId];
                SoundEffectInstance soundEffectInstance = _soundEffectInstances.ContainsKey(soundEffect)
                    ? _soundEffectInstances[soundEffect]
                    : soundEffect.CreateInstance();

                _audioListener.Position = perceiverPosition;
                _audioEmitter.Position = sourcePosition;
                soundEffectInstance.Apply3D(_audioListener, _audioEmitter);
                soundEffectInstance.Play();

                if (!_soundEffectInstances.ContainsKey(soundEffect))
                    _soundEffectInstances.Add(soundEffect, soundEffectInstance);
                _audioListener.Position = Vector3.Zero;
                _audioEmitter.Position = Vector3.Zero;
            }
            else
            {
                throw new Exception($"Sound effect with id '{soundEffectId}' does not exist");
            }
        }

        public void Dispose()
        {
            foreach (Song song in Songs.Values)
            {
                song.Dispose();
            }

            foreach (var soundEffectInstance in _soundEffectInstances)
            {
                soundEffectInstance.Value.Dispose();
            }

            foreach (SoundEffect soundEffect in SoundEffects.Values)
            {
                soundEffect.Dispose();
            }
        }
    }
}