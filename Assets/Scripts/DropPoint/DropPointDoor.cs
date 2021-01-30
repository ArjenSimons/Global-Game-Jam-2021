using BWolf.Utilities;
using System.Collections;
using UnityEngine;

public class DropPointDoor : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField]
    private float closeDelay = 1.0f;

    [SerializeField]
    private float timeClosed = 0.5f;

    [SerializeField]
    private Vector3 closePosition = default;

    [SerializeField]
    private Vector3 closeScale = default;

    [SerializeField]
    private float closeTime = 1.0f;

    [Header("Scene References")]
    [SerializeField]
    private ItemDropRecognizer itemDropRecognizer = null;

    [SerializeField]
    private ItemManager itemManager = null;

    private Vector3 openScale;
    private Vector3 openPosition;

    private bool droppingOfItem;

    private void Start()
    {
        openScale = transform.localScale;
        openPosition = transform.localPosition;
    }

    public void OnDropOffButtonPressed()
    {
        if (!droppingOfItem)
        {
            StartCoroutine(DropOffItemBehindDoor());
        }
    }

    private IEnumerator DropOffItemBehindDoor()
    {
        droppingOfItem = true;

        yield return new WaitForSeconds(closeDelay);
        yield return Close();

        DropOffItem();

        yield return new WaitForSeconds(timeClosed);
        yield return Open();

        droppingOfItem = false;
    }

    private IEnumerator Close()
    {
        LerpValue<Vector3> scaling = new LerpValue<Vector3>(openScale, closeScale, closeTime);
        LerpValue<Vector3> moving = new LerpValue<Vector3>(openPosition, closePosition, closeTime);
        while (scaling.Continue() && moving.Continue())
        {
            transform.localScale = Vector3.Lerp(scaling.start, scaling.end, scaling.perc);
            transform.localPosition = Vector3.Lerp(moving.start, moving.end, moving.perc);
            yield return null;
        }
    }

    private IEnumerator Open()
    {
        LerpValue<Vector3> scaling = new LerpValue<Vector3>(closeScale, openScale, closeTime);
        LerpValue<Vector3> moving = new LerpValue<Vector3>(closePosition, openPosition, closeTime);
        while (scaling.Continue() && moving.Continue())
        {
            transform.localScale = Vector3.Lerp(scaling.start, scaling.end, scaling.perc);
            transform.localPosition = Vector3.Lerp(moving.start, moving.end, moving.perc);
            yield return null;
        }
    }

    private void DropOffItem()
    {
        if (itemDropRecognizer.HasRecogizedItem)
        {
            if (itemManager.ReturnItem(itemDropRecognizer.RecognizedItem.GetComponentInParent<Item>().LostItem))
            {
                Destroy(itemDropRecognizer.RecognizedItem);
                //TODO: Add feedback for dropping off an item
            }
        }
    }
}