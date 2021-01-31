// Created By: Unity Open Project @ https://github.com/UnityTechnologies/open-project-1
//----------------------------------

using UnityEngine;
using UnityEngine.Audio;

namespace BWolf.Utilities.AudioPlaying
{
    /// <summary>
    /// Contains data for configuring an AudioSource component
    /// </summary>
    [CreateAssetMenu(fileName = "AudioConfig", menuName = "Audio/Configuration")]
    public class AudioConfigurationSO : ScriptableObject
    {
        [Header("Management")]
        [SerializeField]
        private AudioMixerGroup outputAudioMixerGroup = null;

        [SerializeField, Range(0.0f, 1.0f)]
        private float spatialBlend = 1f;

        [Header("Sound")]
        [Range(0.0f, 1.0f)]
        public float volume = 1.0f;

        public void ApplyToSource(AudioSource source)
        {
            source.outputAudioMixerGroup = outputAudioMixerGroup;
            source.spatialBlend = spatialBlend;
            source.volume = volume;
        }
    }
}