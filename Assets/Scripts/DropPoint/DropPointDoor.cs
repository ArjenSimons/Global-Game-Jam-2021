using UnityEngine;

public class DropPointDoor : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField]
    private float closeDelay = 1.0f;

    [SerializeField]
    private float timeClosed = 0.5f;

    [SerializeField]
    private Vector3 closePosition = new Vector3(0, 1.05f, 0.1f);

    [SerializeField]
    private float closeTime = 1.0f;

    [Header("Scene References")]
    [SerializeField]
    private ItemDropRecognizer itemDropRecognizer = null;

    [SerializeField]
    private ItemManager itemManager = null;

    private Vector3 openPosition;

    private bool droppingOfItem;

    private void Start()
    {
        openPosition = transform.localPosition;
    }

    public void OnDropOffButtonPressed()
    {
        if (!droppingOfItem)
        {
            droppingOfItem = true;

            var seq = LeanTween.sequence();
            seq.append(closeDelay);
            seq.append(LeanTween.moveLocal(gameObject, closePosition, closeTime));
            seq.append(() =>
            {
                DropOffItem();
            });
            seq.append(closeTime);
            seq.append(LeanTween.moveLocal(gameObject, openPosition, closeTime));
            seq.append(() =>
            {
                droppingOfItem = false;
            });
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