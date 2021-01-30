using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

enum fillerType
{
    Trolley,
    Suitcase
}

enum fillerColor
{
    Black
}

public class ItemManager : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField]
    private float startDelay = 2.0f;

    [SerializeField]
    private float dropInterval = 30.0f;

    [SerializeField]
    private int dropRate = 5;

    [SerializeField]
    private float dropBurstInterval = 0.125f;

    [SerializeField] [Range(0f, 100f)]
    private float fillerDropPercentage = 40;

    [Header("Scene References")]
    [SerializeField] private ThrashShute thrashShute = null;

    [Header("Project References")]
    [SerializeField] private ItemTypes itemTypes = null;

    [SerializeField] private ItemColors itemColors = null;

    [Header("Channel Broadcasting on")]
    [SerializeField]
    private LostItemChannel itemRequestedChannel = null;

    private List<LostItem> fillerItems = new List<LostItem>();
    private List<LostItem> spawnableItems = new List<LostItem>();
    private List<LostItem> droppedItems = new List<LostItem>();
    private List<LostItem> requestedItems = new List<LostItem>();

    public List<LostItem> DroppedItems => droppedItems;
    public List<LostItem> RequestedItems => requestedItems;

    private void Start()
    {
        foreach (ItemType type in itemTypes.Types)
        {
            foreach (ItemColor color in itemColors.Colors)
            {
                LostItem lostItem = new LostItem(type, color);

                if (Enum.GetNames(typeof(fillerType)).Contains(type.DisplayName.ToString()) && Enum.GetNames(typeof(fillerColor)).Contains(color.DisplayName.ToString()))
                {
                    fillerItems.Add(lostItem);
                }
                else
                {
                    spawnableItems.Add(lostItem);
                }
            }
        }

        itemRequestedChannel.OnEventRaised += RequestItem;

        StartCoroutine(DropAtInterval());
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Return))
        {
            DropRandomItem();
        }
    }

    private bool DropRandomItem()
    {

        if (UnityEngine.Random.Range(0f, 100f) < fillerDropPercentage)
        {
            LostItem item = GetRandomFillerItem();
            thrashShute.DropItem(item);
        }
        else
        {
            if (spawnableItems.Count <= 0)
            {
                return false;
            }

            LostItem item = GetRandomItem();
            spawnableItems.Remove(item);
            droppedItems.Add(item);
            thrashShute.DropItem(item);

            //TODO: make sure this happens at random interfals
            itemRequestedChannel.RaiseEvent(item);
        }

        return true;
    }

    public void RequestItem(LostItem item)
    {
        droppedItems.Remove(item);
        requestedItems.Add(item);
    }

    public bool ReturnItem(LostItem item)
    {
        requestedItems.Remove(item);
        spawnableItems.Add(item);

        return true;
    }

    private LostItem GetRandomItem()
    {
        int randomIndex = UnityEngine.Random.Range(0, spawnableItems.Count);
        LostItem item = spawnableItems[randomIndex];

        return item;
    }

    private LostItem GetRandomFillerItem()
    {
        int randomIndex = UnityEngine.Random.Range(0, fillerItems.Count);
        LostItem item = fillerItems[randomIndex];

        return item;
    }

    private IEnumerator DropAtInterval()
    {
        yield return new WaitForSeconds(startDelay);

        while (true)
        {
            yield return DropItemsInBurst();
            yield return new WaitForSeconds(dropInterval);
        }
    }

    private IEnumerator DropItemsInBurst()
    {
        int count = dropRate;
        while (count != 0 && DropRandomItem())
        {
            yield return new WaitForSeconds(dropBurstInterval);
            count--;
        }
    }
}