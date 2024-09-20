using System;
using UnityEngine;

namespace SetUps
{
    public enum AudioMixerGroup
    {
        Android,
        Enemy,
        Bullet,
    }

    [Serializable]
    public class AudioSetUp
    {
        public string soundName;
        public AudioMixerGroup group;
        public AudioClip clip;
        public float volume = 1f;
        public float pitch = 1f;
    }
}