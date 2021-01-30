using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

internal enum fillerType
{
    Trolley,
    Suitcase
}

internal enum fillerColor
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

    [SerializeField, Min(1)]
    private int formAmountFirstDrop = 2;

    [SerializeField, Min(1)]
    private float formAmountIncreaseRatio = 1.1f;

    [SerializeField, Range(0f, 100f)]
    private float fillerDropPercentage = 40;

    [Header("Scene References")]
    [SerializeField] private ThrashShute thrashShute = null;

    [Header("Project References")]
    [SerializeField] private ItemTypes itemTypes = null;

    [SerializeField] private ItemColors itemColors = null;

    [SerializeField] private GameFlowSettings gameFlow = null;

    [Header("Channel Broadcasting on")]
    [SerializeField]
    private LostItemChannel itemRequestedChannel = null;

    private List<LostItem> fillerItems = new List<LostItem>();
    private List<LostItem> spawnableItems = new List<LostItem>();
    private List<LostItem> droppedItems = new List<LostItem>();
    private List<LostItem> requestedItems = new List<LostItem>();

    public List<LostItem> DroppedItems => droppedItems;
    public List<LostItem> RequestedItems => requestedItems;

    private bool dropping;
    private float nextFormBundleAmount;

    private void Awake()
    {
        gameFlow.OnGameStart += OnGameStart;
    }

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
        nextFormBundleAmount = formAmountFirstDrop;
    }

    private void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyUp(KeyCode.Return))
        {
            DropRandomItem();
        }
#endif
    }

    private void OnGameStart()
    {
        if (!dropping)
        {
            dropping = true;
            StartCoroutine(DropAtInterval());
        }
        else
        {
            Debug.LogWarning("the item manager already started dropping items");
        }
    }

    private bool DropRandomItem(bool forceSpawnNonFiller = false)
    {
        if (forceSpawnNonFiller || UnityEngine.Random.Range(0f, 100f) > fillerDropPercentage)
        {
            if (spawnableItems.Count <= 0)
            {
                Debug.LogWarning("There are not enough spawnable items to drop a new item!");
                return false;
            }

            LostItem item = GetRandomItem();
            spawnableItems.Remove(item);
            droppedItems.Add(item);
            thrashShute.DropItem(item);
        }
        else
        {
            LostItem item = GetRandomFillerItem();
            thrashShute.DropItem(item);
        }

        return true;
    }

    public void PrintSomeForms()
    {
        // Spawn extra luggage if there isn't enough for the amount of new forms
        int amountOfForms = (int)nextFormBundleAmount;
        if (nextFormBundleAmount > droppedItems.Count)
        {
            for (int i = 0; i < amountOfForms - droppedItems.Count; i++)
            {
                DropRandomItem(true);
            }
            
            amountOfForms = Mathf.Min(amountOfForms, droppedItems.Count);
        }

        // Print next few forms
        for (int i = 0; i < amountOfForms; i++)
        {
            LostItem item = droppedItems[UnityEngine.Random.Range(0, droppedItems.Count)];
            itemRequestedChannel.RaiseEvent(item);
        }

        // Increase amount of next form drop
        nextFormBundleAmount *= formAmountIncreaseRatio;
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