using BWolf.Utilities;
using BWolf.Utilities.AudioPlaying;
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

    [SerializeField]
    private float minRangeForInteraction = 2.0f;

    [SerializeField]
    private float minRangeForObjectTrigger = 0.5f;

    [Header("Sound")]
    [SerializeField]
    private AudioCueSO buttonPressCue = null;

    [SerializeField]
    private AudioCueSO mechanicalButtonPressCue = null;

    [SerializeField]
    private AudioConfigurationSO config = null;

    [Header("Channel Broadcasting on")]
    [SerializeField]
    private AudioRequestChannelSO channel = null;

    [Header("Events")]
    [SerializeField]
    private UnityEvent OnPress = null;

    private bool hasFocus;
    private bool isBeingPressed;

    private float sqrMinRangeForInteraction;
    private float sqrMinRangeForObjectTrigger;

    private Transform tfMainCamera;

    public bool AllowPress { get; set; } = true;

    private void Start()
    {
        tfMainCamera = Camera.main.transform;

        sqrMinRangeForInteraction = minRangeForInteraction * minRangeForInteraction;
        sqrMinRangeForObjectTrigger = minRangeForObjectTrigger * minRangeForObjectTrigger;
    }

    private void Update()
    {
        if (hasFocus && Input.GetMouseButtonDown(0) && AllowPress)
        {
            TryPress();
        }
    }

    private void OnMouseEnter()
    {
        if (CameraIsInRange())
        {
            hasFocus = true;
        }
    }

    private void OnMouseExit()
    {
        hasFocus = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Item item = collision.gameObject.GetComponentInParent<Item>();
        if (item != null && item.CanTriggerDropOffButton && IsInObjectTriggerRange(item.transform.position))
        {
            TryPress();
        }
    }

    private bool IsInObjectTriggerRange(Vector3 position)
    {
        return (position - transform.position).sqrMagnitude < sqrMinRangeForObjectTrigger;
    }

    private bool CameraIsInRange()
    {
        return (tfMainCamera.position - transform.position).sqrMagnitude < sqrMinRangeForInteraction;
    }

    private void TryPress()
    {
        if (!isBeingPressed)
        {
            StartCoroutine(DoPress());
        }
    }

    private IEnumerator DoPress()
    {
        isBeingPressed = true;
        channel.RaiseEvent(config, buttonPressCue, transform.position);
        channel.RaiseEvent(config, mechanicalButtonPressCue, transform.position);

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