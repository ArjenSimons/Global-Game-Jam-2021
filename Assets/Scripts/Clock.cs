using System;
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

    [Header("Scene References")]
    [SerializeField]
    private TMP_Text textTime = null;

    [Header("Project References")]
    [SerializeField]
    private GameFlowSettings gameFlow = null;

    [Header("Events")]
    public UnityEvent onWorkdayOver;

    private float currentTime;

    private const int MINUTES_IN_HOUR = 60;
    private const float FIFTY_PERCENT = 0.5f;
    private const float SINGLE_DIGIT_LIMIT = 10;

    private int CurrentHours => Mathf.FloorToInt(currentTime);
    private int CurrentMinutes => Mathf.FloorToInt((currentTime % 1) * MINUTES_IN_HOUR);
    private bool HalfWayToNextSecond => (Time.realtimeSinceStartup % 1) >= FIFTY_PERCENT;

    private bool timeIsPassing;

    private void Awake()
    {
        gameFlow.OnGameStart += OnGameStart;
        gameFlow.OnGameEnd += OnGameEnd;
    }

    private void Start()
    {
        SetStartTime();
    }

    private void Update()
    {
        if (timeIsPassing)
        {
            currentTime += Time.deltaTime / durationOfOneHour;
        }

        SetTime();

        if (CurrentHours == endTime && CurrentMinutes == 0)
        {
            onWorkdayOver.Invoke();
        }

        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            onWorkdayOver.Invoke();
        }
    }

    private void OnGameStart()
    {
        timeIsPassing = true;
    }

    private void OnGameEnd()
    {
        SetStartTime();
        timeIsPassing = false;
    }

    private void SetStartTime()
    {
        currentTime = startTime;
    }

    private void SetTime()
    {
        string hours = (CurrentHours < SINGLE_DIGIT_LIMIT ? "0" : string.Empty) + CurrentHours;
        string minutes = (CurrentMinutes < SINGLE_DIGIT_LIMIT ? "0" : string.Empty) + CurrentMinutes;
        string connector = HalfWayToNextSecond ? ":" : " ";
        textTime.text = hours + connector + minutes;
    }
}