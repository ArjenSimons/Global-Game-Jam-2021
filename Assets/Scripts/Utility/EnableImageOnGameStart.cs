using System;
using System.Collections;
using System.Collections.Generic;
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
    }

    private void OnGameStart()
    {
        img.enabled = true;
    }
}