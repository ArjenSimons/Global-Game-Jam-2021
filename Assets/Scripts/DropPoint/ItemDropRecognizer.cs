using UnityEngine;
using System.Collections.Generic;

public class ItemDropRecognizer : MonoBehaviour
{
    public List<Item> Items { get; } = new List<Item>();
    public List<Form> Forms { get; } = new List<Form>();

    public bool HasRecogizedItems
    {
        get { return Items.Count != 0 || Forms.Count != 0; }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PickupableItem"))
        {
            Item item = other.GetComponent<Item>();
            if (item != null && !Items.Contains(item))
            {
                Items.Add(item);
            }
            else
            {
                Form form = other.GetComponent<Form>();
                if (form != null && !Forms.Contains(form))
                {
                    Forms.Add(form);
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("PickupableItem"))
        {
            Item item = other.GetComponent<Item>();
            if (item != null && Items.Contains(item))
            {
                Items.Remove(item);
            }
            else
            {
                Form form = other.GetComponent<Form>();
                if (form != null && Forms.Contains(form))
                {
                    Forms.Remove(form);
                }
            }
        }
    }
}