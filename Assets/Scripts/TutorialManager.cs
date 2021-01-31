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

    [Header("Voice Over Audio")]
    [SerializeField]
    private AudioClip[] clips = null;

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
                Text6();
            }
        }

        if (waitForGrabTrolley)
        {
            if (trolleyGrabbable.IsGrabbed)
            {
                waitForGrabTrolley = false;
                Text8();
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
        subtitles.PlayAudio(clips[0]);
    }

    private void Text2()
    {
        subtitles.OnTextDone.RemoveListener(Text2);
        subtitles.OnTextDone.AddListener(Text3);
        subtitles.ShowText("I'm here to help you on your first day.", 2f);
    }

    private void Text3()
    {
        subtitles.OnTextDone.RemoveListener(Text3);
        subtitles.OnTextDone.AddListener(Text4);
        subtitles.ShowText("I'm working remotely, because, well, you know!", 3.8f);
    }

    private void Text4()
    {
        subtitles.OnTextDone.RemoveListener(Text4);
        subtitles.OnTextDone.AddListener(Text5);
        subtitles.ShowText("Since it's your first day, let me give you your instructions:", 3f);
    }

    private void Text5()
    {
        subtitles.OnTextDone.RemoveListener(Text5);
        subtitles.OnTextDone.AddListener(EnableHands);
        subtitles.ShowText("Next to your desk is a board.", 2f);
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

    private void Text6()
    {
        marker3DForm.TrackedObject = null;
        subtitles.ShowText("Alright, as the form says, someone lost their red trolley.", 4.5f);
        subtitles.OnTextDone.AddListener(Text7);
        subtitles.PlayAudio(clips[1]);
    }

    private void Text7()
    {
        subtitles.OnTextDone.RemoveListener(Text7);
        subtitles.OnTextDone.AddListener(AllowGrabTrolley);
        subtitles.ShowText("Grab the red trolley in your other hand!", 3f);
    }

    private void AllowGrabTrolley()
    {
        subtitles.OnTextDone.RemoveListener(AllowGrabTrolley);
        trolleyGrabbable = itemSet.GetMatchWithLostItem(formSet.GetFirstFormInSet().ItemDisplaying).GetComponent<Grabbable>();
        waitForGrabTrolley = true;
        marker3DTrolley.TrackedObject = trolleyGrabbable.transform;
    }

    private void Text8()
    {
        print("text8");
        StartCoroutine(EnableButton());
        subtitles.ShowText("Put them BOTH in the droppoint and press the button.", 3f);
        subtitles.PlayAudio(clips[2]);
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
            subtitles.PlayAudio(clips[3]);
        }
    }

    private void MissingForm2()
    {
        subtitles.OnTextDone.RemoveListener(MissingForm2);
        subtitles.ShowText("Both the form AND the trolley! Try again!", 3f);
        StartCoroutine(itemManager.RespawnTutorialItem());
    }

    private void ReceivedAPoint()
    {
        if (gameFlowSettings.IsInTutorial)
        {
            incrementIncorrectFormsChannel.OnEventRaised -= MissingForm;
            incrementCorrectFormsChannel.OnEventRaised -= ReceivedAPoint;
            subtitles.OnTextDone.AddListener(Text9);
            subtitles.ShowText("That's it, that's all it is!", 2f);
            subtitles.PlayAudio(clips[4]);
            marker3DButton.TrackedObject = null;
            marker3DDropPoint.TrackedObject = null;
        }
    }

    private void Text9()
    {
        subtitles.OnTextDone.RemoveListener(Text9);
        subtitles.OnTextDone.AddListener(Text10);
        subtitles.ShowText("One more thing: The computer will print new forms every hour.", 4f);
    }

    private void Text10()
    {
        subtitles.OnTextDone.RemoveListener(Text10);
        subtitles.OnTextDone.AddListener(Text11);
        subtitles.ShowText("It'll get worse towards the end of the day. Just see how much you'll get done, okay?", 3.8f);
    }

    private void Text11()
    {
        subtitles.OnTextDone.RemoveListener(Text11);
        StartCoroutine(EndTutorial());
        subtitles.ShowText("Let's get to work! I'm gonna open the chute.", 3f);
    }

    private IEnumerator EndTutorial()
    {
        yield return new WaitForSeconds(delayBeforeStartGame);

        isBusy = false;

        gameFlowSettings.RaiseGameStateEvent(GameStateChange.TutorialEnded);
        gameFlowSettings.RaiseGameStateEvent(GameStateChange.GameStarted);
    }
}