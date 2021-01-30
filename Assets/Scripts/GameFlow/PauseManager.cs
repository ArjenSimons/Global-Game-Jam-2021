using UnityEngine;
using UnityEngine.Events;

public class PauseManager : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField]
    private float fadeTime = 0.5f;

    [SerializeField]
    private float fadeTargetAlpha = 0.5f;

    [Header("Project References")]
    [SerializeField]
    private GameFlowSettings gameFlow = null;

    [SerializeField]
    private BoolChannel pauseChannel = null;

    [Header("Scene References")]
    [SerializeField]
    private GameFlowBackgroundFader fader = null;

    [SerializeField]
    private GameObject pauseMenu = null;

    private bool isPaused;

    private void Update()
    {
        if (!gameFlow.GameHasStarted)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isPaused)
            {
                ActivatePause();
            }
            else
            {
                EndPause();
            }
        }
    }

    private void ActivatePause()
    {
        isPaused = true;
        pauseChannel.RaiseEvent(isPaused);

        fader.FadeToTarget(fadeTargetAlpha, fadeTime).setOnComplete(() =>
        {
            Time.timeScale = 0.0f;
            pauseMenu.SetActive(true);
        });
    }

    public void OnQuitButtonClick()
    {
        Application.Quit();
    }

    public void EndPause()
    {
        isPaused = false;

        Time.timeScale = 1.0f;

        pauseMenu.SetActive(false);
        fader.SetTransparant();

        pauseChannel.RaiseEvent(isPaused);
    }
}