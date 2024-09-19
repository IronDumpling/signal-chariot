using System;
using UnityEngine;

namespace SetUps
{
    [Serializable]
    public class AudioSetUp
    {
        public string soundName;
        public AudioClip clip;
        public float volume = 1f;
        public float pitch = 1f;
    }
}