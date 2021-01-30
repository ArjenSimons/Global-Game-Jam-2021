using UnityEngine;
using UnityEngine.UI;

public class ImageFader : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField]
    private float fadeTime = 1.0f;

    private Image img;

    private void Awake()
    {
        img = GetComponent<Image>();
        SetTransparant();
        Fade();
    }

    private void SetTransparant()
    {
        Color color = img.color;
        color.a = 0;
        img.color = color;
    }

    public void Fade()
    {
        LTSeq seq = LeanTween.sequence();
        seq.append(LeanTween.value(1, 0, fadeTime).setOnUpdate(perc =>
        {
            Color color = img.color;
            color.a = perc;
            img.color = color;
        }));
    }
}