using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class Subtitles : MonoBehaviour
{
    [SerializeField]
    private TMP_Text textSubtitles = null;

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

    private IEnumerator DisableText(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        textSubtitles.enabled = false;
        OnTextDone.Invoke();
    }
}
