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

        gameFlow.OnTutorialStart += OnTutorialStart;
        gameFlow.OnGameEnd += OnGameEnd;

        pauseChannel.OnEventRaised += OnGamePause;
    }

    private void OnGamePause(bool value)
    {
        EnableImage(!value);
    }

    private void OnGameEnd(bool quitted)
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