using System.Collections.Generic;
using UnityEngine;

using SetUps;
using Utils.Common;

namespace Audios
{
    public class AudioManager : MonoSingleton<AudioManager>
    {
        [SerializeField] private AudioSource m_source;
        [SerializeField] private SetUp m_setUp;
        private AudioSetUp[] m_soundEffects;

        private void Awake()
        {
            m_soundEffects = m_setUp.audioLibrary.ToArray();
        }

        public void PlaySound(string soundName)
        {
            AudioSetUp sound = GetSoundByName(soundName);
            if (sound != null)
            {
                m_source.volume = sound.volume;
                m_source.pitch = sound.pitch;
                m_source.clip = sound.clip;
                // m_source.PlayOneShot(sound.clip);
                m_source.Play();
            }
            else
                Debug.LogWarning($"Sound: {soundName} not found!");
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