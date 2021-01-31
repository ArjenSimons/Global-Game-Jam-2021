using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoresManager : MonoBehaviour
{
    [Header("Scene References")]
    [SerializeField]
    private Computer computer;

    [Header("Project References")]
    [SerializeField]
    private GameFlowSettings gameFlow = null;

    [Header("Channel Broadcasting on")]
    [SerializeField]
    private EmptyChannel incrementCorrectFormsChannel = null;

    [SerializeField]
    private EmptyChannel incrementIncorrectFormsChannel = null;

    [SerializeField]
    private EmptyChannel scoresUpdatedChannel = null;

    private int correctForms = 0;
    private int incorrectForms = 0;

    public int CorrectForms => correctForms;
    public int IncorrectForms => incorrectForms;

    private void Awake()
    {
        gameFlow.OnGameStateChanged += OnGameStateChanged;
    }

    private void OnGameStateChanged(GameStateChange gameStateChange)
    {
        switch (gameStateChange)
        {
            case GameStateChange.GameStarted:
                ResetCounts();
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
        ResetCounts();
    }

    private void ResetCounts()
    {
        correctForms = 0;
        incorrectForms = 0;
        scoresUpdatedChannel.RaiseEvent();
    }

    private void Start()
    {
        incrementCorrectFormsChannel.OnEventRaised += IncrementCorrectForms;
        incrementIncorrectFormsChannel.OnEventRaised += IncrementIncorrectForms;
    }

    private void IncrementCorrectForms()
    {
        correctForms += 1;
        scoresUpdatedChannel.RaiseEvent();
    }

    private void IncrementIncorrectForms()
    {
        incorrectForms += 1;
        scoresUpdatedChannel.RaiseEvent();
    }
}