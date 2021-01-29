using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{

    [SerializeField] private ThrashShute thrashShute = null;
    [SerializeField] private ItemTypes itemTypes = null;
    [SerializeField] private ItemColors itemColors = null;

    private List<LostItem> spawnableItems = new List<LostItem>();
    private List<LostItem> droppedItems = new List<LostItem>();
    private List<LostItem> requestedItems = new List<LostItem>();

    public List<LostItem> AvailableItems => spawnableItems;
    public List<LostItem> DroppedItems => droppedItems;

    public event Action OnItemDrop;

    private void Start()
    {
        foreach(ItemType type in itemTypes.Types)
        {
            foreach(ItemColor color in itemColors.Colors)
            {
                LostItem lostItem = new LostItem(type, color);
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.D))
        {
            DropRandomItem();
        }
    }

    private void DropRandomItem()
    {
        LostItem item = GetRandomItem();
        spawnableItems.Remove(item);
        droppedItems.Add(item);
        thrashShute.DropItem(item.ItemType.PrefabItem);
    }

    private void RequestItem(LostItem item)
    {
        droppedItems.Remove(item);
        requestedItems.Add(item);
    }

    private void ReturnItem(LostItem item)
    {
        requestedItems.Remove(item);
        spawnableItems.Add(item);
    }

    private LostItem GetRandomItem()
    {
        int randomIndex = UnityEngine.Random.Range(0, spawnableItems.Count);
        LostItem item = spawnableItems[randomIndex];

        return item;
    }
}
