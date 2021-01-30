using System.Collections.Generic;
using UnityEngine;

public class DropPointDoor : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField]
    private float closeDelay = 1.0f;

    [SerializeField]
    private Vector3 closePosition = new Vector3(0, 1.05f, 0.1f);

    [SerializeField]
    private float closeTime = 1.0f;

    [Header("Scene References")]
    [SerializeField]
    private ItemDropRecognizer itemDropRecognizer = null;

    [SerializeField]
    private ItemManager itemManager = null;

    [Header("Project References")]
    [SerializeField]
    private FormSet formSet = null;

    [Header("Channel Broadcasting on")]
    [SerializeField]
    private LostItemChannel itemRequestedChannel = null;

    private Vector3 openScale;

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
                DropOffItems();
            });
            seq.append(closeTime);
            seq.append(LeanTween.moveLocal(gameObject, openPosition, closeTime));
            seq.append(() =>
            {
                droppingOfItem = false;
            });
        }
    }

    private void DropOffItems()
    {
        if (itemDropRecognizer.HasRecogizedItems)
        {
            List<Item> items = itemDropRecognizer.Items;
            List<Form> forms = itemDropRecognizer.Forms;

            //destroy each item after checking if there is a form that matches it
            for (int i = items.Count - 1; i >= 0; i--)
            {
                Item item = items[i];
                bool wasSuccesfull = false;

                for (int j = 0; j < forms.Count; j++)
                {
                    Form form = forms[j];
                    LostItem lostItem = item.LostItem;
                    if (lostItem.Equals(form.ItemDisplaying))
                    {
                        Destroy(form.gameObject);
                        forms.Remove(form);
                        formSet.Remove(form);

                        wasSuccesfull = true;

                        break;
                    }
                }

                OnItemDelivered(item, wasSuccesfull);
                Destroy(item.gameObject);
                items.Remove(item);
            }

            //remove leftover forms
            for (int i = forms.Count - 1; i >= 0; i--)
            {
                Form form = forms[i];

                //request new form based since matching item is still in the scene
                itemRequestedChannel.RaiseEvent(form.ItemDisplaying);

                Destroy(form.gameObject);
                forms.Remove(form);
                formSet.Remove(form);
            }
        }
    }

    private void OnItemDelivered(Item item, bool succes)
    {
        if (!succes)
        {
            Form matchingForm = formSet.GetMatchWithLostItem(item.LostItem);
            if (matchingForm != null)
            {
                Destroy(matchingForm.gameObject);
            }
            else
            {
                Debug.LogWarning("an item was destroyed without a matching form found in the world :: this is not intended!");
            }
        }

        //TODO: implement scoring system on computer
    }
}