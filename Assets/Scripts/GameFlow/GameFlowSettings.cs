using System;
using UnityEngine;

[CreateAssetMenu(menuName = "GameFlow/Settings")]
public class GameFlowSettings : ScriptableObject
{
    public bool StartAtComputer = true;
    public bool GameHasStarted = false;

    public Action OnTutorialStart;
    public Action OnGameStart;

    public Action OnGameRestart;
    public Action OnStartGameRestart;

    public Action<bool> OnGameEnd;

    private void OnEnable()
    {
        GameHasStarted = !StartAtComputer;
    }

    public void RaiseTutorialStartEvent()
    {
        if (OnTutorialStart != null)
        {
            OnTutorialStart();
            GameHasStarted = false;
        }
        else
        {
            Debug.LogWarning("The tutorial start event was raised but no one was listening. Make sure the player is in" +
                "the scene to respond");
        }
    }

    public void RaiseGameStartEvent()
    {
        if (OnGameStart != null)
        {
            OnGameStart();
            GameHasStarted = true;
        }
        else
        {
            Debug.LogWarning("The game start event was raised but no one was listening. Make sure the player is in" +
                "the scene to respond");
        }
    }

    public void RaiseRestartEvent()
    {
        if (OnGameRestart != null)
        {
            OnGameRestart();
        }
        else
        {
            Debug.LogWarning("The game restart event was raised but no one was listening. Make sure the player is in" +
                "the scene to respond");
        }
    }

    public void RaiseStartRestartEvent()
    {
        if (OnStartGameRestart != null)
        {
            OnStartGameRestart();
        }
        else
        {
            Debug.LogWarning("The game restart event was raised but no one was listening. Make sure the player is in" +
                "the scene to respond");
        }
    }

    public void RaiseGameEndEvent(bool quitted)
    {
        if (OnGameEnd != null)
        {
            OnGameEnd(quitted);
            GameHasStarted = false;
        }
        else
        {
            Debug.LogWarning("The game end event was raised but no one was listening. Make sure the player is in" +
                "the scene to respond");
        }
    }
}

public enum GameState
{
    StartMenu,
    GameStarted,
    GameEnded,
}