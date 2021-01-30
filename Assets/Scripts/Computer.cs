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

    [Header("Scene references")]
    [SerializeField]
    private ScoresManager scoreManager = null;

    [Header("Channel Broadcasting on")]
    [SerializeField]
    private EmptyChannel scoresUpdatedChannel = null;

    private StartMenuButton focusedButton;

    private void Start()
    {
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
    }

    private void Update()
    {
        if (!gameFlow.GameHasStarted)
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
                        SetupGameplayFlow();
                        break;

                    case StartMenuButton.Exit:
                        OnExitButtonPress();
                        break;
                }
            }
        }
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
        textCorrectForms.gameObject.SetActive(false);
        textIncorrectForms.gameObject.SetActive(false);

        btnStart.gameObject.SetActive(true);
        btnExit.gameObject.SetActive(true);

        SetButtonFocus(StartMenuButton.Start);
    }

    private void SetupGameplayFlow()
    {
        btnStart.gameObject.SetActive(false);
        btnExit.gameObject.SetActive(false);

        textCorrectForms.gameObject.SetActive(true);
        textIncorrectForms.gameObject.SetActive(true);

        SetTexts();

        gameFlow.RaiseGameStartEvent();
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