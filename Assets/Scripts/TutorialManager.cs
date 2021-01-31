using System.Collections;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    [SerializeField]
    private GameFlowSettings gameFlowSettings = null;

    [SerializeField]
    private EmptyChannel incrementCorrectFormsChannel = null;

    [SerializeField]
    private EmptyChannel incrementIncorrectFormsChannel = null;

    [SerializeField]
    private DropOffButton dropOffButton = null;

    [SerializeField]
    private Subtitles subtitles = null;

    [SerializeField]
    private PlayerController playerController = null;

    [SerializeField]
    private ItemManager itemManager = null;

    [SerializeField]
    private FormSet formSet = null;

    [SerializeField]
    private float delayBeforeStartTutorial = 2f;

    [SerializeField]
    private float delayBeforeStartGame = 2f;

    [SerializeField]
    private TrackPos marker3DForm = null;

    [SerializeField]
    private TrackPos marker3DButton = null;

    private bool waitForGrabForm;
    private Grabbable formGrabbable;

    private bool isBusy;

    private void Start()
    {
        gameFlowSettings.OnGameStateChanged += OnGameStateChanged;

        incrementCorrectFormsChannel.OnEventRaised += ReceivedAPoint;
        incrementIncorrectFormsChannel.OnEventRaised += MissingForm;
    }

    private void Update()
    {
        if (waitForGrabForm)
        {
            if (formGrabbable.IsGrabbed)
            {
                waitForGrabForm = false;
                Text5();
            }
        }
    }

    private void OnGameStateChanged(GameStateChange gameStateChange)
    {
        switch (gameStateChange)
        {
            case GameStateChange.TutorialStarted:
                StartTutorial();
                break;

            case GameStateChange.OnGameEnd:
                OnGameEnd();
                break;

            default:
                break;
        }
    }

    private void OnGameEnd()
    {
        if (isBusy)
        {
            isBusy = false;

            StopAllCoroutines();

            dropOffButton.AllowPress = true;
            playerController.CanGrab = true;

            subtitles.ForceDisable();

            gameFlowSettings.RaiseGameStateEvent(GameStateChange.TutorialEnded);
        }
    }

    private void StartTutorial()
    {
        StartCoroutine(BeginTutorial());
        dropOffButton.AllowPress = false;
        playerController.CanGrab = false;
    }

    private IEnumerator BeginTutorial()
    {
        isBusy = true;

        yield return new WaitForSeconds(delayBeforeStartTutorial);
        Text1();
    }

    private void Text1()
    {
        subtitles.OnTextDone.AddListener(Text2);
        subtitles.ShowText("Hey Bob, it's me, Bill. It's gonna be a hard first day since noone is there to help you!", 5f);
    }

    private void Text2()
    {
        subtitles.OnTextDone.RemoveListener(Text2);
        subtitles.OnTextDone.AddListener(Text3);
        subtitles.ShowText("I'll give you some quick instructions on how to do your job properly.", 4f);
    }

    private void Text3()
    {
        subtitles.OnTextDone.RemoveListener(Text3);
        subtitles.OnTextDone.AddListener(EnableHands);
        subtitles.ShowText("Next to your desk is your board. If new forms are printed out, throw 'em on the board.", 5f);
    }

    private void EnableHands()
    {
        subtitles.OnTextDone.RemoveListener(EnableHands);
        subtitles.ShowText("I left a form from yesterday. Grab it.", 3f);
        playerController.CanGrab = true;
        formGrabbable = formSet.GetFirstFormInSet().GetComponent<Grabbable>();
        waitForGrabForm = true;
        marker3DForm.TrackedObject = formSet.GetFirstFormInSet().transform;
    }

    private void Text5()
    {
        marker3DForm.TrackedObject = null;
        subtitles.ShowText("Alright, as the form says, someone lost their red trolley.", 4f);
        subtitles.OnTextDone.AddListener(Text6);
    }

    private void Text6()
    {
        subtitles.OnTextDone.RemoveListener(Text6);
        subtitles.OnTextDone.AddListener(Text7);
        subtitles.ShowText("Your job is simple: Put the lost item WITH the matching form in the hole, then hit the button.", 5f);
    }

    private void Text7()
    {
        subtitles.OnTextDone.RemoveListener(Text7);
        StartCoroutine(EnableButton());
        subtitles.ShowText("Go ahead and do it.", 2f);
    }

    private IEnumerator EnableButton()
    {
        yield return new WaitForSeconds(1f);
        dropOffButton.AllowPress = true;
        marker3DButton.TrackedObject = dropOffButton.transform;
    }

    private void MissingForm()
    {
        subtitles.OnTextDone.AddListener(MissingForm2);
        subtitles.ShowText("Not like that, Bob! You need to put in TWO things.", 3f);
    }

    private void MissingForm2()
    {
        subtitles.OnTextDone.RemoveListener(MissingForm2);
        subtitles.ShowText("Both the form AND the trolley! Try again!", 3f);
        itemManager.RespawnTutorialItem();
    }

    private void ReceivedAPoint()
    {
        incrementIncorrectFormsChannel.OnEventRaised -= MissingForm;
        incrementCorrectFormsChannel.OnEventRaised -= ReceivedAPoint;
        subtitles.OnTextDone.AddListener(ReceivedAPoint2);
        subtitles.ShowText("That's it, well done!", 2f);
        marker3DButton.TrackedObject = null;
    }

    private void ReceivedAPoint2()
    {
        subtitles.OnTextDone.RemoveListener(ReceivedAPoint2);
        StartCoroutine(EndTutorial());
        subtitles.ShowText("Let's get to work! I'm gonna open the chute.", 4f);
    }

    private IEnumerator EndTutorial()
    {
        yield return new WaitForSeconds(delayBeforeStartGame);

        isBusy = false;

        gameFlowSettings.RaiseGameStateEvent(GameStateChange.TutorialEnded);
        gameFlowSettings.RaiseGameStateEvent(GameStateChange.GameStarted);
    }
}