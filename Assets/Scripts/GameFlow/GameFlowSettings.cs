using System;
using UnityEngine;

[CreateAssetMenu(menuName = "GameFlow/Settings")]
public class GameFlowSettings : ScriptableObject
{
    public bool StartAtComputer = true;
    public bool GameHasStarted = false;

    public Action OnGameStart;

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
}