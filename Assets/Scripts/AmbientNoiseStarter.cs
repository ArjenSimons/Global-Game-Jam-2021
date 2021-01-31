using BWolf.Utilities.AudioPlaying;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbientNoiseStarter : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField]
    private AudioCueSO ambientCue = null;

    [SerializeField]
    private AudioConfigurationSO config = null;

    [Header("Channel broadcasting on")]
    [SerializeField]
    private AudioRequestChannelSO channel = null;

    private void Start()
    {
        Debug.Log("Play");
        channel.RaiseEvent(config, ambientCue, Vector3.zero);
    }
}
