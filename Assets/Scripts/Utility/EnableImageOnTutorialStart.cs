using UnityEngine;
using UnityEngine.UI;

public class EnableImageOnTutorialStart : MonoBehaviour
{
    [SerializeField]
    private GameFlowSettings gameFlow = null;

    private Image img;

    private void Awake()
    {
        img = GetComponent<Image>();
        img.enabled = false;

        gameFlow.OnTutorialStart += OnTutorialStart;
        gameFlow.OnGameEnd += OnGameEnd;
    }

    private void OnGameEnd()
    {
        img.enabled = false;
    }

    private void OnTutorialStart()
    {
        img.enabled = true;
    }
}