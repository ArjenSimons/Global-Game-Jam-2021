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
    private ItemSet itemSet = null;

    [SerializeField]
    private float delayBeforeStartTutorial = 2f;

    [SerializeField]
    private float delayBeforeStartGame = 2f;

    [SerializeField]
    private TrackPos marker3DForm = null;

    [SerializeField]
    private TrackPos marker3DTrolley = null;

    [SerializeField]
    private TrackPos marker3DDropPoint = null;

    [SerializeField]
    private TrackPos marker3DButton = null;

    [SerializeField]
    private Transform DropPointTransform = null;

    private bool waitForGrabForm;
    private bool waitForGrabTrolley;
    private Grabbable formGrabbable;
    private Grabbable trolleyGrabbable;

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
        if (waitForGrabTrolley)
        {
            if (trolleyGrabbable.IsGrabbed)
            {
                waitForGrabForm = false;
                Text7();
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
        subtitles.ShowText("Hey Bob, it's me, Bill.", 2f);
    }

    private void Text2()
    {
        subtitles.OnTextDone.RemoveListener(Text2);
        subtitles.OnTextDone.AddListener(Text3);
        subtitles.ShowText("I'm working remotely, because, well, you know!", 3f);
    }

    private void Text3()
    {
        subtitles.OnTextDone.RemoveListener(Text3);
        subtitles.OnTextDone.AddListener(Text4);
        subtitles.ShowText("Since it's your first day, here are your instructions:", 2.5f);
    }

    private void Text4()
    {
        subtitles.OnTextDone.RemoveListener(Text4);
        subtitles.OnTextDone.AddListener(EnableHands);
        subtitles.ShowText("Check the board next to your desk.", 1.5f);
    }

    private void EnableHands()
    {
        subtitles.OnTextDone.RemoveListener(EnableHands);
        subtitles.ShowText("I left a form on it yesterday, grab it.", 3f);
        playerController.CanGrab = true;
        formGrabbable = formSet.GetFirstFormInSet().GetComponent<Grabbable>();
        waitForGrabForm = true;
        marker3DForm.TrackedObject = formSet.GetFirstFormInSet().transform;
    }

    private void Text5()
    {
        marker3DForm.TrackedObject = null;
        subtitles.ShowText("Alright, as the form says, someone lost their red trolley.", 2.5f);
        subtitles.OnTextDone.AddListener(Text6);
    }

    private void Text6()
    {
        subtitles.OnTextDone.RemoveListener(Text6);
        subtitles.ShowText("Grab the red trolley in your other hand!", 2f);
        trolleyGrabbable = itemSet.GetMatchWithLostItem(formSet.GetFirstFormInSet().ItemDisplaying).GetComponent<Grabbable>();
        waitForGrabTrolley = true;
        marker3DTrolley.TrackedObject = trolleyGrabbable.transform;
    }

    private void Text7()
    {
        StartCoroutine(EnableButton());
        subtitles.ShowText("Put them in the droppoint and press the button", 2f);
        marker3DDropPoint.TrackedObject = DropPointTransform;
        marker3DTrolley.TrackedObject = null;
    }

    private IEnumerator EnableButton()
    {
        yield return new WaitForSeconds(1f);
        dropOffButton.AllowPress = true;
        marker3DButton.TrackedObject = dropOffButton.transform;
    }

    private void MissingForm()
    {
        if (gameFlowSettings.IsInTutorial)
        {
            subtitles.OnTextDone.AddListener(MissingForm2);
            subtitles.ShowText("Not like that, Bob! You need to put in TWO things.", 3f);
        }
    }

    private void MissingForm2()
    {
        subtitles.OnTextDone.RemoveListener(MissingForm2);
        subtitles.ShowText("Both the form AND the trolley! Try again!", 3f);
        itemManager.RespawnTutorialItem();
    }

    private void ReceivedAPoint()
    {
        if (gameFlowSettings.IsInTutorial)
        {
            incrementIncorrectFormsChannel.OnEventRaised -= MissingForm;
            incrementCorrectFormsChannel.OnEventRaised -= ReceivedAPoint;
            subtitles.OnTextDone.AddListener(Text8);
            subtitles.ShowText("That's it, well done!", 2f);
            marker3DButton.TrackedObject = null;
            marker3DDropPoint.TrackedObject = null;
        }
    }

    private void Text8()
    {
        subtitles.OnTextDone.RemoveListener(Text8);
        subtitles.OnTextDone.AddListener(Text9);
        subtitles.ShowText("One more thing: The computer will print new forms every hour.", 3f);
    }

    private void Text9()
    {
        subtitles.OnTextDone.RemoveListener(Text9);
        subtitles.OnTextDone.AddListener(Text10);
        subtitles.ShowText("It'll get worse towards the end of the day. Just see how much you'll get done, okay?", 5f);
    }

    private void Text10()
    {
        subtitles.OnTextDone.RemoveListener(Text10);
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