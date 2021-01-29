using BWolf.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DropOffButton : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField]
    private float pressFactor = 4f;

    [SerializeField]
    private float pressSpeed = 2f;

    [Header("Events")]
    [SerializeField]
    private UnityEvent OnPress = null;

    private bool hasFocus;
    private bool isBeingPressed;

    private void Update()
    {
        if (!isBeingPressed && hasFocus && Input.GetMouseButtonDown(0))
        {
            StartCoroutine(Press());
        }
    }

    private void OnMouseEnter()
    {
        hasFocus = true;
    }

    private void OnMouseExit()
    {
        hasFocus = false;
    }

    private IEnumerator Press()
    {
        isBeingPressed = true;

        float scaleY = transform.localScale.y;
        PingPongValue press = new PingPongValue(scaleY / pressFactor, scaleY, 1, pressSpeed, false, 1.0f);
        while (press.Continue())
        {
            Vector3 newScale = transform.localScale;
            newScale.y = press.value;
            transform.localScale = newScale;
            yield return null;
        }

        OnPress.Invoke();

        isBeingPressed = false;
    }
}