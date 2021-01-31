using BWolf.Utilities.AudioPlaying;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class Clock : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField, Range(0, 23)]
    private int startTime = 9;

    [SerializeField, Range(0, 23)]
    private int endTime = 17;

    [SerializeField]
    private float durationOfOneHour = 10;

    [Header("Sound")]
    [SerializeField]
    private AudioCueSO endGameAudiooCue = null;

    [SerializeField]
    private AudioCueSO digitalAlarmCue = null;

    [SerializeField]
    private AudioConfigurationSO config = null;

    [Header("Channel Broadcasting on")]
    [SerializeField]
    private AudioRequestChannelSO channel = null;

    [Header("Scene References")]
    [SerializeField]
    private TMP_Text textTime = null;

    [Header("Project References")]
    [SerializeField]
    private GameFlowSettings gameFlow = null;

    [SerializeField]
    private BoolChannel workdayOverChannel = null;

    [Header("Events")]
    public UnityEvent onHalfHourPassed;

    public UnityEvent onHourPassed;

    private float currentTime;
    private int _currentMinutes;

    private const int MINUTES_IN_HOUR = 60;
    private const float FIFTY_PERCENT = 0.5f;
    private const float SINGLE_DIGIT_LIMIT = 10;

    private int CurrentHours => Mathf.FloorToInt(currentTime);

    private int CurrentMinutes
    {
        get
        {
            return _currentMinutes;
        }
        set
        {
            // Exit if value is already the same
            if (value == CurrentMinutes)
            {
                return;
            }

            _currentMinutes = value;

            CheckEvents();
        }
    }

    private bool HalfWayToNextSecond => (Time.realtimeSinceStartup % 1) >= FIFTY_PERCENT;

    private bool timeCanIncrease;
    private bool workdayOver;

    private void Awake()
    {
        SetStartTime();
    }

    private void Start()
    {
        gameFlow.OnGameStateChanged += OnGameStateChanged;
        //onWorkdayOver.AddListener(PlayEndGameAlarm);
    }

    private void OnGameStateChanged(GameStateChange gameStateChange)
    {
        switch (gameStateChange)
        {
            case GameStateChange.GameStarted:
                StartCoroutine(OnGameStart());
                break;

            case GameStateChange.GameRestarted:
                OnGameRestart();
                break;

            case GameStateChange.OnGameEnd:
                OnGameEnd();
                break;

            default:
                break;
        }
    }

    private void Update()
    {
        if (!workdayOver)
        {
            //if there is work to be done and time is passing, update time
            if (timeCanIncrease)
            {
                currentTime += Time.deltaTime / durationOfOneHour;
                CurrentMinutes = Mathf.FloorToInt((currentTime % 1) * MINUTES_IN_HOUR);
            }

            if (CurrentHours == endTime && CurrentMinutes == 0)
            {
                OnWorkDayOver(false);
            }
        }

        SetTime();

#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            OnWorkDayOver(false);
        }
#endif
    }

    private IEnumerator OnGameStart()
    {
        yield return new WaitForSeconds(3.5f);
        timeCanIncrease = true;
        workdayOver = false;
        CheckEvents();
    }

    private void CheckEvents()
    {
        if (CurrentMinutes == 0 || CurrentMinutes == 30)
        {
            onHalfHourPassed.Invoke();
        }
        if (CurrentMinutes == 0)
        {
            if (CurrentHours != 9 && CurrentHours != endTime)
            {
                channel.RaiseEvent(config, digitalAlarmCue, transform.position);
            }
            onHourPassed.Invoke();
        }
        if (CurrentHours == endTime - 1 && CurrentMinutes == 57)
        {
            PlayEndGameAlarm();
        }
        if (CurrentHours == endTime && CurrentMinutes == 0)
        {
            OnWorkDayOver(false);
        }
    }

    private void OnWorkDayOver(bool quitted)
    {
        timeCanIncrease = false;
        workdayOver = true;
        workdayOverChannel.RaiseEvent(quitted);
    }

    private void OnGameRestart()
    {
        SetStartTime();
        SetTime();
    }

    private void OnGameEnd()
    {
        timeCanIncrease = false;
        if (gameFlow.GameEndState == GameEndState.PauseMenuQuit)
        {
            OnGameRestart();
        }
    }

    private void SetStartTime()
    {
        currentTime = startTime;
        CurrentMinutes = Mathf.FloorToInt((currentTime % 1) * MINUTES_IN_HOUR);
    }

    private void SetTime()
    {
        string hours = (CurrentHours < SINGLE_DIGIT_LIMIT ? "0" : string.Empty) + CurrentHours;
        string minutes = (CurrentMinutes < SINGLE_DIGIT_LIMIT ? "0" : string.Empty) + CurrentMinutes;
        string connector = HalfWayToNextSecond ? ":" : " ";
        textTime.text = hours + connector + minutes;
    }

    private void PlayEndGameAlarm()
    {
        channel.RaiseEvent(config, endGameAudiooCue, transform.position);
    }
}