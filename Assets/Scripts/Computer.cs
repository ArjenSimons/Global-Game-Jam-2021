using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Computer : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField]
    private GameFlowSettings gameFlow = null;

    [Header("Gameplay Feedback references")]
    [SerializeField]
    private TMP_Text textCorrectForms = null;

    [SerializeField]
    private TMP_Text textIncorrectForms = null;

    [Header("Start Menu Flow references")]
    [SerializeField]
    private Button btnStart = null;

    [SerializeField]
    private Button btnExit = null;

    [Header("End Menu Flow references")]
    [SerializeField]
    private Button btnBackToMenu = null;

    [SerializeField]
    private TMP_Text textEndingCorrectForms = null;

    [SerializeField]
    private TMP_Text textEndingIncorrectForms = null;

    [Header("Scene references")]
    [SerializeField]
    private ScoresManager scoreManager = null;

    [Header("Channel Broadcasting on")]
    [SerializeField]
    private EmptyChannel scoresUpdatedChannel = null;

    private StartMenuButton focusedButton;

    private bool isInEndGameScreen;

    private void Start()
    {
        ClearScreen();

        if (gameFlow.StartAtComputer)
        {
            //show menu
            SetupStartMenuFlow();
        }
        else
        {
            SetupGameplayFlow();
        }

        scoresUpdatedChannel.OnEventRaised += SetTexts;

        gameFlow.OnGameEnd += OnGameEnd;
    }

    private void OnGameEnd()
    {
        ClearScreen();
        SetupEndGameFlow();
        ApplyGamePlayResultsToEndGameText();

        isInEndGameScreen = true;
    }

    private void Update()
    {
        if (!gameFlow.GameHasStarted)
        {
            if (isInEndGameScreen)
            {
                CheckForEndMenuInput();
            }
            else
            {
                CheckForStartMenuInput();
            }
        }
    }

    private void CheckForEndMenuInput()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            ClearScreen();
            SetupStartMenuFlow();

            isInEndGameScreen = false;
        }
    }

    private void CheckForStartMenuInput()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            CycleThroughStartMenu(CycleDirection.Down);
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            CycleThroughStartMenu(CycleDirection.Up);
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            switch (focusedButton)
            {
                case StartMenuButton.Start:
                    ClearScreen();
                    SetupGameplayFlow();
                    break;

                case StartMenuButton.Exit:
                    OnExitButtonPress();
                    break;
            }
        }
    }

    private void ClearScreen()
    {
        btnStart.gameObject.SetActive(false);
        btnExit.gameObject.SetActive(false);

        textCorrectForms.gameObject.SetActive(false);
        textIncorrectForms.gameObject.SetActive(false);

        btnBackToMenu.gameObject.SetActive(false);

        textEndingCorrectForms.gameObject.SetActive(false);
        textEndingIncorrectForms.gameObject.SetActive(false);
    }

    private void ApplyGamePlayResultsToEndGameText()
    {
        textEndingCorrectForms.text = textCorrectForms.text;
        textEndingIncorrectForms.text = textIncorrectForms.text;
    }

    private void OnExitButtonPress()
    {
        Application.Quit();
    }

    private void CycleThroughStartMenu(CycleDirection direction)
    {
        int value = (int)direction;
        int current = (int)focusedButton;

        int newValue = current + value;
        int buttonCount = System.Enum.GetValues(typeof(CycleDirection)).Length;
        if (newValue < 0)
        {
            newValue = buttonCount - 1;
        }
        else if (newValue == buttonCount)
        {
            newValue = 0;
        }

        SetButtonFocus((StartMenuButton)newValue);
    }

    private void SetButtonFocus(StartMenuButton button)
    {
        ResetButtonInteractability();

        switch (button)
        {
            case StartMenuButton.Start:
                btnStart.interactable = true;
                break;

            case StartMenuButton.Exit:
                btnExit.interactable = true;
                break;
        }

        focusedButton = button;
    }

    private void ResetButtonInteractability()
    {
        btnStart.interactable = false;
        btnExit.interactable = false;
    }

    private void SetupStartMenuFlow()
    {
        btnStart.gameObject.SetActive(true);
        btnExit.gameObject.SetActive(true);

        SetButtonFocus(StartMenuButton.Start);
    }

    private void SetupGameplayFlow()
    {
        textCorrectForms.gameObject.SetActive(true);
        textIncorrectForms.gameObject.SetActive(true);

        SetTexts();

        gameFlow.RaiseGameStartEvent();
    }

    private void SetupEndGameFlow()
    {
        btnBackToMenu.gameObject.SetActive(true);

        textEndingCorrectForms.gameObject.SetActive(true);
        textEndingIncorrectForms.gameObject.SetActive(true);
    }

    private void SetTexts()
    {
        textCorrectForms.text = scoreManager.CorrectForms + " forms completed";
        textIncorrectForms.text = scoreManager.IncorrectForms + " mistakes";
    }

    private enum StartMenuButton
    {
        Start = 0,
        Exit,
    }

    private enum CycleDirection
    {
        Up = 1,
        Down = -1
    }
}