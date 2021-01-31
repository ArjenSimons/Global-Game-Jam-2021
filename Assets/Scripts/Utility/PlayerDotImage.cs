using UnityEngine;
using UnityEngine.UI;

public class PlayerDotImage : MonoBehaviour
{
    [SerializeField]
    private GameFlowSettings gameFlow = null;

    [SerializeField]
    private BoolChannel pauseChannel = null;

    private Image img;

    private void Awake()
    {
        img = GetComponent<Image>();
        img.enabled = false;

        gameFlow.OnGameStateChanged += OnGameStateChanged;

        pauseChannel.OnEventRaised += OnGamePause;
    }

    private void OnGamePause(bool value)
    {
        EnableImage(!value);
    }

    private void OnGameStateChanged(GameStateChange gameStateChange)
    {
        switch (gameStateChange)
        {
            case GameStateChange.TutorialStarted:
                OnTutorialStart();
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
        EnableImage(false);
    }

    private void OnTutorialStart()
    {
        EnableImage(true);
    }

    private void EnableImage(bool value)
    {
        img.enabled = value;
    }
}