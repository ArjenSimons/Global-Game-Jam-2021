using BWolf.Utilities;
using System.Collections;
using System.Collections.Generic;
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
            StartCoroutine(DropOffItemsBehindDoor());
        }
    }

    private IEnumerator DropOffItemsBehindDoor()
    {
        droppingOfItem = true;

        yield return new WaitForSeconds(closeDelay);
        yield return Close();

        DropOffItems();

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
                Destroy(form.gameObject);
                forms.Remove(form);
            }
        }
    }

    private void OnItemDelivered(Item item, bool succes)
    {
        print($"delivered item: {item} : {(succes ? "succesfully" : "unsuccesfully")}");
        //TODO: implement scoring system on computer
    }
}