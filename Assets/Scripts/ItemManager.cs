using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using BWolf.Utilities.AudioPlaying;

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

    [Header("Tutorial Settings")]
    [SerializeField]
    private LostItem tutorialLostItem = default;

    [SerializeField]
    private GameObject prefabForm = null;

    [SerializeField]
    private Vector3 tutorialItemPosition = Vector3.zero;

    [SerializeField]
    private Vector3 tutorialFormPosition = Vector3.zero;

    [SerializeField]
    private Vector3 tutorialItemRotation = Vector3.zero;

    [Header("Sound")]
    [SerializeField]
    private AudioCueSO audioCue = null;

    [SerializeField]
    private AudioConfigurationSO config = null;

    [Header("Scene References")]
    [SerializeField] private ThrashShute thrashShute = null;

    [Header("Project References")]
    [SerializeField] private ItemTypes itemTypes = null;

    [SerializeField] private ItemColors itemColors = null;

    [SerializeField] private GameFlowSettings gameFlow = null;

    [SerializeField] private ItemSet itemSet = null;

    [SerializeField] private FormSet formSet = null;

    [Header("Channel Broadcasting on")]
    [SerializeField]
    private LostItemChannel itemRequestedChannel = null;

    [SerializeField]
    private AudioRequestChannelSO channel = null;

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
        gameFlow.OnTutorialStart += OnTutorialStart;
        gameFlow.OnGameStart += OnGameStart;
        gameFlow.OnGameRestart += OnGameRestart;
        gameFlow.OnGameEnd += OnGameEnd;

        nextFormBundleAmount = formAmountFirstDrop;
    }

    private void PopulateLists()
    {
        fillerItems.Clear();
        spawnableItems.Clear();
        droppedItems.Clear();
        requestedItems.Clear();

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
    }

    private void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyUp(KeyCode.Return) && dropping)
        {
            DropRandomItem();
        }
#endif
    }

    private void OnTutorialStart()
    {
        spawnableItems.Add(tutorialLostItem);

        // spawn this item and it's form. Reposition them
        DropRandomItem(true);
        LostItem lostItem = droppedItems[UnityEngine.Random.Range(0, droppedItems.Count)];
        SpawnTutorialForm(lostItem);
        Form form = formSet.GetMatchWithLostItem(lostItem);
        Item item = itemSet.GetMatchWithLostItem(lostItem);

        form.transform.position = tutorialFormPosition;
        item.transform.position = tutorialItemPosition;
        item.transform.rotation = Quaternion.Euler(tutorialItemRotation);
    }

    private void SpawnTutorialForm(LostItem lostItem)
    {
        Form form = Instantiate(prefabForm, Vector3.zero, transform.rotation).GetComponent<Form>();
        form.SetText("John Doe", lostItem);
        form.EnablePaper();
        form.transform.position = tutorialFormPosition;
    }

    public void RespawnTutorialItem()
    {
        DropRandomItem(true);
        LostItem lostItem = droppedItems[UnityEngine.Random.Range(0, droppedItems.Count)];
        itemRequestedChannel.RaiseEvent(lostItem);
    }

    private void OnGameStart()
    {
        if (!dropping)
        {
            dropping = true;
            PopulateLists();
            StartCoroutine(DropAtInterval());
        }
        else
        {
            Debug.LogWarning("the item manager already started dropping items");
        }
    }

    private void OnGameRestart()
    {
        itemSet.DestroyAll();
        formSet.DestroyAll();
    }

    private void OnGameEnd(bool quitted)
    {
        StopAllCoroutines();

        fillerItems.Clear();
        spawnableItems.Clear();
        droppedItems.Clear();
        requestedItems.Clear();

        dropping = false;

        if (quitted)
        {
            OnGameRestart();
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
            int amount = amountOfForms - droppedItems.Count;
            for (int i = 0; i < amount; i++)
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
            yield return DropItemsInBurst(dropRate);
            yield return new WaitForSeconds(dropInterval);
        }
    }

    private IEnumerator DropItemsInBurst(int count)
    {
        channel.RaiseEvent(config, audioCue, thrashShute.transform.position);

        yield return new WaitForSeconds(2.8f);

        while (count != 0 && DropRandomItem())
        {
            yield return new WaitForSeconds(dropBurstInterval);
            count--;
        }
    }
}