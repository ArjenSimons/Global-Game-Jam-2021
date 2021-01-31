using System.Collections;
using UnityEngine;

public class CommentaryManager : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField, Min(0), Tooltip("How many seconds until the next event may trigger a message")]
    private float minDelay = 3f;

    [SerializeField, Min(0), Tooltip("How many seconds before the next event will 100% trigger a message")]
    private float minDelayBeforeForce = 10f;

    [SerializeField, Range(0f, 1f), Tooltip("Chance for the next event to trigger a message")]
    private float commentChance = 0.5f;

    [SerializeField]
    private EmptyChannel incrementCorrectFormsChannel = null;

    [SerializeField]
    private EmptyChannel incrementIncorrectFormsChannel = null;

    [Header("References")]
    [SerializeField]
    private Subtitles subtitles = null;

    [SerializeField]
    private CommentaryLibrary commentaryLibrary = null;

    [SerializeField]
    private GameFlowSettings gameFlowSettings = null;

    private bool minDelayPassed;
    private bool minDelayBeforeForcePassed;
    private Coroutine CountDownRoutine;

    public bool CanComment { get; set; }
    private bool ShouldComment
    {
        get
        {
            // Auto-false if we can't comment or the min delay hasn't passed
            if (!CanComment || !minDelayPassed)
            {
                return false;
            }

            // Auto-true if the min delay before force passed
            if (minDelayBeforeForcePassed)
            {
                return true;
            }

            // Chance to comment
            return Random.value < commentChance;
        }
    }

    private void Start()
    {
        incrementCorrectFormsChannel.OnEventRaised += CommentOnIncrementCorrectForms;
        gameFlowSettings.OnGameStart += EnableCommentary;
        gameFlowSettings.OnGameEnd += DisableCommentary;
        minDelayPassed = true;
        minDelayBeforeForcePassed = true;
    }

    private void EnableCommentary()
    {
        CanComment = true;
    }

    private void DisableCommentary(bool quitted)
    {
        CanComment = false;
    }

    private void CommentOnIncrementCorrectForms()
    {
        if (ShouldComment)
        {
            Commentary commentary = commentaryLibrary.GetCommentary();
            if (commentary == null)
            {
                return;
            }

            TriggerMessage(commentary);
        }
    }

    private void TriggerMessage(Commentary commentary)
    {
        subtitles.ShowText(commentary.Text, commentary.SpeakDuration);
        subtitles.OnTextDone.AddListener(TextDone);
    }

    private void TextDone()
    {
        subtitles.OnTextDone.RemoveListener(TextDone);
        if (CountDownRoutine != null)
        {
            StopCoroutine(CountDownRoutine);
        }
        CountDownRoutine = StartCoroutine(CountDownDelays());
    }

    private IEnumerator CountDownDelays()
    {
        minDelayPassed = false;
        minDelayBeforeForcePassed = false;

        yield return new WaitForSeconds(minDelay);
        minDelayPassed = true;

        yield return new WaitForSeconds(minDelayBeforeForce - minDelay);
        minDelayBeforeForcePassed = true;
    }

    private void OnValidate()
    {
        if (minDelayBeforeForce < minDelay)
        {
            minDelayBeforeForce = minDelay;
        }
    }
}
