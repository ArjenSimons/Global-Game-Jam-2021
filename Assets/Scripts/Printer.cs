using UnityEngine;
using System.Collections.Generic;
using BWolf.Utilities.AudioPlaying;

public class Printer : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField]
    private float tweenDistance = 0.38f;

    [SerializeField]
    private float tweenDuration = 3f;

    [Header("Sound")]
    [SerializeField]
    private AudioCueSO audioCue = null;

    [SerializeField]
    private AudioConfigurationSO config = null;

    [Header("Project References")]
    [SerializeField]
    private GameObject prefabForm = null;

    [SerializeField]
    private FormSet forms = null;

    [Header("Scene References")]
    [SerializeField]
    private Transform printSpawn = null;

    [Header("Channel Listening to")]
    [SerializeField]
    private LostItemChannel itemRequestedChannel = null;

    [SerializeField]
    private AudioRequestChannelSO channel = null;

    private Queue<LostItem> queuedItems = new Queue<LostItem>();

    private bool printing;

    private void Start()
    {
        itemRequestedChannel.OnEventRaised += OnItemRequested;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            Print("John Doe", "Is anyone reading this");
        }
    }

    private void OnItemRequested(LostItem lostItem)
    {
        if (!printing)
        {
            //if we are not printing, print the lost item immediately
            Print(lostItem);
        }
        else
        {
            //if the printer is already printing, enque the lost item
            queuedItems.Enqueue(lostItem);
        }
    }

    private void OnFormPrinted(Form form)
    {
        form.EnablePaper();

        if (!TryDequeLostItem())
        {
            //if no lost items can be dequed, stop printing
            printing = false;
        }
    }

    private bool TryDequeLostItem()
    {
        if (queuedItems.Count != 0)
        {
            //if there are queued items to be dequed, dequeue a lost item to be printed
            Print(queuedItems.Dequeue());
            return true;
        }

        return false;
    }

    public void Print(string name, string text)
    {
        printing = true;
        channel.RaiseEvent(config, audioCue, Vector3.zero);

        Form form = Instantiate(prefabForm, printSpawn.position, transform.rotation).GetComponent<Form>();
        form.SetText(name, "", "", text);
        forms.Add(form);
        LeanTween.move(form.gameObject, printSpawn.position - printSpawn.forward * tweenDistance, tweenDuration)
            .setOnComplete(() => OnFormPrinted(form));
    }

    public void Print(LostItem lostItem)
    {
        printing = true;
        channel.RaiseEvent(config, audioCue, Vector3.zero);

        Form form = Instantiate(prefabForm, printSpawn.position, transform.rotation).GetComponent<Form>();
        form.SetText("John Doe", lostItem);
        forms.Add(form);
        LeanTween.move(form.gameObject, printSpawn.position - printSpawn.forward * tweenDistance, tweenDuration)
            .setOnComplete(() => OnFormPrinted(form));
    }
}