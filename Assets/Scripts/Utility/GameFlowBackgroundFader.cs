using System;
using UnityEngine;
using UnityEngine.UI;

public class GameFlowBackgroundFader : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField]
    private float fadeTime = 1.0f;

    [Header("Project Reference")]
    [SerializeField]
    private GameFlowSettings gameFlow = null;

    private Image img;

    private void Awake()
    {
        img = GetComponent<Image>();
        SetTransparant();
        FadeOut();

        gameFlow.OnStartGameRestart += OnStartGameRestart;
    }

    private void OnStartGameRestart()
    {
        FadeIn().setOnComplete(() =>
        {
            gameFlow.RaiseRestartEvent();
            FadeOut();
        });
    }

    public void SetTransparant()
    {
        Color color = img.color;
        color.a = 0;
        img.color = color;
    }

    public LTDescr FadeToTarget(float value, float time)
    {
        float clamped = Mathf.Clamp01(value);
        float current = img.color.a;

        LTSeq seq = LeanTween.sequence();
        LTDescr fade = LeanTween.value(current, clamped, time).setOnUpdate(perc =>
        {
            Color color = img.color;
            color.a = perc;
            img.color = color;
        });

        seq.append(fade);

        return fade;
    }

    public void FadeOut()
    {
        LTSeq seq = LeanTween.sequence();
        seq.append(LeanTween.value(1, 0, fadeTime).setOnUpdate(perc =>
        {
            Color color = img.color;
            color.a = perc;
            img.color = color;
        }));
    }

    public void OnWorkDayOver()
    {
        FadeIn().setOnComplete(() =>
        {
            FadeOut();
            gameFlow.RaiseGameEndEvent();
        });
    }

    public LTDescr FadeIn()
    {
        LTSeq seq = LeanTween.sequence();
        LTDescr fadeIn = LeanTween.value(0, 1, fadeTime).setOnUpdate(perc =>
        {
            Color color = img.color;
            color.a = perc;
            img.color = color;
        });

        seq.append(fadeIn);

        return fadeIn;
    }
}