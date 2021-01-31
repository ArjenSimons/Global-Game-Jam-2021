using BWolf.Utilities.AudioPlaying;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class Subtitles : MonoBehaviour
{
    [Header("Scene References")]
    [SerializeField]
    private TMP_Text textSubtitles = null;

    [SerializeField]
    private Transform tfVoiceOverCamera = null;

    [Header("Project References")]
    [SerializeField]
    private AudioClipRequestChannelSO audioClipRequestChannel = null;

    [SerializeField]
    private AudioConfigurationSO audioConfig = null;

    [Header("Events")]
    public UnityEvent OnTextDone;

    private Coroutine disableTextRoutine;

    private void Start()
    {
        textSubtitles.enabled = false;
    }

    public void ShowText(string text, float seconds)
    {
        textSubtitles.text = text;
        textSubtitles.enabled = true;

        if (disableTextRoutine != null)
        {
            StopCoroutine(disableTextRoutine); //TODO: Check if OntextDone should be invoked if the previous text ended early, maybe make it a UnityEvent<bool>
        }

        disableTextRoutine = StartCoroutine(DisableText(seconds));
    }

    public void PlayAudio(AudioClip clip)
    {
        if (clip != null)
        {
            audioClipRequestChannel.RaiseRequest(clip, audioConfig, false, tfVoiceOverCamera.position);
        }
        else
        {
            Debug.LogError("Can't play subtitle voice over audio :: Clip is null");
        }
    }

    public void ForceDisable()
    {
        StopCoroutine(disableTextRoutine);
        textSubtitles.text = string.Empty;
        textSubtitles.enabled = false;
    }

    private IEnumerator DisableText(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        textSubtitles.enabled = false;
        OnTextDone.Invoke();
    }
}