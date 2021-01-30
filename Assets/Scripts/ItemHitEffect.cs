using BWolf.Utilities.AudioPlaying;
using UnityEngine;

public class ItemHitEffect : MonoBehaviour
{
    [Header("Sound")]
    [SerializeField]
    private AudioCueSO audioCue = null;

    [SerializeField]
    private AudioConfigurationSO config = null;

    [Header("Channel Broadcasting on")]
    [SerializeField]
    private AudioRequestChannelSO channel = null;

    void OnCollisionEnter(Collision hit)
    {
        Debug.Log(hit.relativeVelocity.magnitude);
        if (hit.relativeVelocity.magnitude > 2.5f || !AudioManager.Instance.IsPlayingAudioCue(audioCue))
        {
            float tempVol = config.volume;

            config.volume = tempVol / 60 * Mathf.Min(hit.relativeVelocity.magnitude, 5);

            channel.RaiseEvent(config, audioCue, transform.position);

            config.volume = tempVol;
        }
    }
}
