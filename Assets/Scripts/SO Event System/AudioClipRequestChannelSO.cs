using BWolf.Utilities.AudioPlaying;
using System;
using UnityEngine;

[CreateAssetMenu(menuName = "SO Event Channels/Audio Clip")]
public class AudioClipRequestChannelSO : ScriptableObject
{
    public Action<AudioClip, AudioConfigurationSO, bool, Vector3> OnRequestRaised;

    public void RaiseRequest(AudioClip clip, AudioConfigurationSO config, bool loop, Vector3 position)
    {
        if (OnRequestRaised != null)
        {
            OnRequestRaised(clip, config, loop, position);
        }
        else
        {
            Debug.LogWarning("an event for an audio clip was raised, but no one was listening ::" +
                "make sure the audio manager is listening");
        }
    }
}