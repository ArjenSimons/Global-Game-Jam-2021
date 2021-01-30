using System;
using UnityEngine;

[CreateAssetMenu(menuName = "GameFlow/Settings")]
public class GameFlowSettings : ScriptableObject
{
    public bool StartAtComputer = true;
    public bool GameHasStarted = false;

    public Action OnGameStart;
    public Action OnGameEnd;

    private void OnEnable()
    {
        GameHasStarted = !StartAtComputer;
    }

    public void RaiseGameStartEvent()
    {
        if (OnGameStart != null)
        {
            OnGameStart();
            GameHasStarted = false;
        }
        else
        {
            Debug.LogWarning("The game start event was raised but no one was listening. Make sure the player is in" +
                "the scene to respond");
        }
    }

    public void RaiseGameEndEvent()
    {
        if (OnGameEnd != null)
        {
            OnGameEnd();
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