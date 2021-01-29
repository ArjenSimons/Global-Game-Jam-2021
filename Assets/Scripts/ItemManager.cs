using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

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

    [Header("Scene References")]
    [SerializeField] private ThrashShute thrashShute = null;

    [Header("Project References")]
    [SerializeField] private ItemTypes itemTypes = null;

    [SerializeField] private ItemColors itemColors = null;

    private List<LostItem> spawnableItems = new List<LostItem>();
    private List<LostItem> droppedItems = new List<LostItem>();
    private List<LostItem> requestedItems = new List<LostItem>();

    public List<LostItem> DroppedItems => droppedItems;
    public List<LostItem> RequestedItems => requestedItems;

    public event Action OnItemDrop;

    private void Start()
    {
        foreach (ItemType type in itemTypes.Types)
        {
            foreach (ItemColor color in itemColors.Colors)
            {
                LostItem lostItem = new LostItem(type, color);
                spawnableItems.Add(lostItem);
            }
        }

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
        if (spawnableItems.Count <= 0)
        {
            return false;
        }

        LostItem item = GetRandomItem();
        spawnableItems.Remove(item);
        droppedItems.Add(item);
        thrashShute.DropItem(item);
        return true;
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