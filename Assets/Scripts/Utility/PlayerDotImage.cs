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

        gameFlow.OnGameStart += OnGameStart;
        gameFlow.OnGameEnd += OnGameEnd;

        pauseChannel.OnEventRaised += OnGamePause;
    }

    private void OnGamePause(bool value)
    {
        EnableImage(!value);
    }

    private void OnGameEnd()
    {
        EnableImage(false);
    }

    private void OnGameStart()
    {
        EnableImage(true);
    }

    private void EnableImage(bool value)
    {
        img.enabled = value;
    }
}