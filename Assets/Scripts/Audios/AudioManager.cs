using System.Collections.Generic;
using UnityEngine;

using SetUps;
using Utils.Common;
using System;

namespace Audios
{
    public class AudioManager : MonoSingleton<AudioManager>
    {
        [SerializeField] private AudioSource m_androidSource; // board, module, mod pickup, android damage
        [SerializeField] private AudioSource m_enemySource; // enemy damage
        [SerializeField] private AudioSource m_bulletSource; // explosion, weapon fire
        [SerializeField] private SetUp m_setUp;
        private AudioSetUp[] m_soundEffects;

        private void Awake()
        {
            m_soundEffects = m_setUp.audioLibrary.ToArray();
        }

        public void PlaySound(string soundName)
        {
            AudioSetUp sound = GetSoundByName(soundName);

            AudioSource source = GetMixerGroup(sound.group);

            if (sound != null)
            {
                source.volume = sound.volume;
                source.pitch = sound.pitch;
                source.clip = sound.clip;
                source.Play();
            }
            else
                Debug.LogWarning($"Sound: {soundName} not found!");
        }

        private AudioSource GetMixerGroup(AudioMixerGroup group)
        {
            switch (group)
            {
                case AudioMixerGroup.Android:
                    return m_androidSource;
                case AudioMixerGroup.Enemy:
                    return m_enemySource;
                case AudioMixerGroup.Bullet:
                    return m_bulletSource;
                default:
                    return null;
            }
        }

        private AudioSetUp GetSoundByName(string name)
        {
            foreach (AudioSetUp sound in m_soundEffects)
                if (sound.soundName == name)
                    return sound;    
            return null;
        }
    }
}