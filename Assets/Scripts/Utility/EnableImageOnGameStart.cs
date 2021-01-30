using UnityEngine;
using UnityEngine.UI;

public class EnableImageOnGameStart : MonoBehaviour
{
    [SerializeField]
    private GameFlowSettings gameFlow = null;

    private Image img;

    private void Awake()
    {
        img = GetComponent<Image>();
        img.enabled = false;

        gameFlow.OnGameStart += OnGameStart;
        gameFlow.OnGameEnd += OnGameEnd;
    }

    private void OnGameEnd()
    {
        img.enabled = false;
    }

    private void OnGameStart()
    {
        img.enabled = true;
    }
}