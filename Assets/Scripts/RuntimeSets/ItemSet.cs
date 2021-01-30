using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "RuntimeSets/Items")]
public class ItemSet : RuntimeSet<Item>
{
    public Item GetMatchWithLostItem(LostItem lostItem)
    {
        foreach (Item item in set)
        {
            if (lostItem.Equals(item.LostItem))
            {
                return item;
            }
        }

        return null;
    }
    public void DestroyAll()
    {
        for (int i = set.Count - 1; i >= 0; i--)
        {
            Destroy(set[i].gameObject);
            set.RemoveAt(i);
        }
    }
}