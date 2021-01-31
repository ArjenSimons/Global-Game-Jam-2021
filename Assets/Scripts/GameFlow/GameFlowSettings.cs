using System;
using UnityEngine;

[CreateAssetMenu(menuName = "GameFlow/Settings")]
public class GameFlowSettings : ScriptableObject
{
    [Header("Developer Settings")]
    public bool StartAtComputer = true;

    public bool SkipTutorial = true;

    [Header("Game State Settings")]
    public bool GameHasStarted = false;

    public bool IsInTutorial = false;

    [Space]
    public GameEndState GameEndState;

    public Action<GameStateChange> OnGameStateChanged;

    private void OnEnable()
    {
        GameHasStarted = false;
        IsInTutorial = false;
    }

    public void RaiseGameStateEvent(GameStateChange change)
    {
        switch (change)
        {
            case GameStateChange.TutorialStarted:
                RaiseTutorialStartEvent();
                break;

            case GameStateChange.TutorialEnded:
                RaiseTutorialEndEvent();
                break;

            case GameStateChange.GameStarted:
                RaiseGameStartEvent();
                break;

            case GameStateChange.GameRestartStarted:
                RaiseRestartEvent();
                break;

            case GameStateChange.GameRestarted:
                RaiseStartRestartEvent();
                break;

            case GameStateChange.OnGameEnd:
                RaiseGameEndEvent();
                break;

            default:
                break;
        }
    }

    private void RaiseTutorialStartEvent()
    {
        if (OnGameStateChanged != null)
        {
            OnGameStateChanged(GameStateChange.TutorialStarted);
            IsInTutorial = true;
        }
        else
        {
            Debug.LogWarning("The tutorial start event was raised but no one was listening. Make sure the player is in" +
                "the scene to respond");
        }
    }

    private void RaiseGameStartEvent()
    {
        if (OnGameStateChanged != null)
        {
            OnGameStateChanged(GameStateChange.GameStarted);
            GameHasStarted = true;
        }
        else
        {
            Debug.LogWarning("The game start event was raised but no one was listening. Make sure the player is in" +
                "the scene to respond");
        }
    }

    private void RaiseRestartEvent()
    {
        if (OnGameStateChanged != null)
        {
            OnGameStateChanged(GameStateChange.GameRestarted);
        }
        else
        {
            Debug.LogWarning("The game restart event was raised but no one was listening. Make sure the player is in" +
                "the scene to respond");
        }
    }

    private void RaiseStartRestartEvent()
    {
        if (OnGameStateChanged != null)
        {
            OnGameStateChanged(GameStateChange.GameRestartStarted);
        }
        else
        {
            Debug.LogWarning("The game restart event was raised but no one was listening. Make sure the player is in" +
                "the scene to respond");
        }
    }

    private void RaiseGameEndEvent()
    {
        if (OnGameStateChanged != null)
        {
            OnGameStateChanged(GameStateChange.OnGameEnd);
            GameHasStarted = false;
            SkipTutorial = true;
        }
        else
        {
            Debug.LogWarning("The game end event was raised but no one was listening. Make sure the player is in" +
                "the scene to respond");
        }
    }

    private void RaiseTutorialEndEvent()
    {
        if (OnGameStateChanged != null)
        {
            OnGameStateChanged(GameStateChange.TutorialEnded);
            IsInTutorial = false;
        }
        else
        {
            Debug.LogWarning("The tutorial end event was raised but no one was listening. Make sure the player is in" +
                "the scene to respond");
        }
    }
}