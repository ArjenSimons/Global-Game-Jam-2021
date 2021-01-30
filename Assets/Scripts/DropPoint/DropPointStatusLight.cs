using System.Collections.Generic;
using UnityEngine;

public class DropPointStatusLight : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField]
    private float lightTime = 1.5f;

    [SerializeField]
    private float flickerTime = 1.0f;

    [Space]
    [SerializeField]
    private Color succesColor = Color.green;

    [SerializeField]
    private Color failedColor = Color.red;

    [SerializeField]
    private Color emptyStatusColor = Color.black;

    [Header("Scene References")]
    [SerializeField]
    private Renderer rendLightBulb = null;

    [SerializeField]
    private Light pointLight = null;

    private Queue<DropStatus> queuedStatusUpdates = new Queue<DropStatus>();

    private bool givingFeedback;

    private void Start()
    {
        rendLightBulb.material.EnableKeyword("EMISSION");
        SetEmptyStatus();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            UpdateStatus(DropStatus.SuccesfullDelivery);
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            UpdateStatus(DropStatus.failedDelivery);
        }
    }

    public void OnItemDelivered(Item item, bool succes)
    {
        UpdateStatus(succes ? DropStatus.SuccesfullDelivery : DropStatus.failedDelivery);
    }

    private void UpdateStatus(DropStatus status)
    {
        if (!givingFeedback)
        {
            //give feedback on status if no feedback is given
            GiveFeedbackOnStatus(status);
        }
        else
        {
            //enque status if feedback is already being given
            queuedStatusUpdates.Enqueue(status);
        }
    }

    private void OnFeedbackEnded()
    {
        if (!TryDequeueStatus())
        {
            SetEmptyStatus();
            givingFeedback = false;
        }
    }

    private bool TryDequeueStatus()
    {
        if (queuedStatusUpdates.Count != 0)
        {
            GiveFeedbackOnStatus(queuedStatusUpdates.Dequeue());
            return true;
        }

        return false;
    }

    private void GiveFeedbackOnStatus(DropStatus status)
    {
        SetEmptyStatus();

        switch (status)
        {
            case DropStatus.SuccesfullDelivery:
                GiveDeliveryFeedback(true);
                break;

            case DropStatus.failedDelivery:
                GiveDeliveryFeedback(false);
                break;
        }
    }

    private void SetEmptyStatus()
    {
        SetColor(emptyStatusColor);
    }

    private void SetColor(Color color)
    {
        rendLightBulb.material.SetColor("_EmissionColor", color);
        pointLight.color = color;
    }

    private void GiveDeliveryFeedback(bool wasSuccesfulDelivery)
    {
        givingFeedback = true;

        Color color = wasSuccesfulDelivery ? succesColor : failedColor;
        LTSeq sequence = LeanTween.sequence();
        sequence.append(LeanTween.value(0, 1, flickerTime).setOnUpdate(perc =>
        {
            SetColor(Color.Lerp(emptyStatusColor, color, perc));
        }));
        sequence.append(lightTime);
        sequence.append(LeanTween.value(0, 1, flickerTime).setOnUpdate(perc =>
        {
            SetColor(Color.Lerp(color, emptyStatusColor, perc));
        }).setOnComplete(OnFeedbackEnded));
    }
}