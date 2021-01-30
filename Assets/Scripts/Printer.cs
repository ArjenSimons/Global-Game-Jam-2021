using UnityEngine;

public class Printer : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField]
    private float tweenDistance = 0.38f;

    [SerializeField]
    private float tweenDuration = 3f;

    [Header("Project References")]
    [SerializeField]
    private GameObject prefabForm = null;

    [Header("Scene References")]
    [SerializeField]
    private Transform printSpawn = null;

    [Header("Channel Listening to")]
    [SerializeField]
    private LostItemChannel itemRequestedChannel = null;

    private void Start()
    {
        itemRequestedChannel.OnEventRaised += Print;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            Print("John Doe", "Is anyone reading this");
        }
    }

    public void Print(string name, string text)
    {
        Form form = Instantiate(prefabForm, printSpawn.position, transform.rotation).GetComponent<Form>();
        form.SetText(name, "", "", text);
        LeanTween.move(form.gameObject, printSpawn.position - printSpawn.forward * tweenDistance, tweenDuration).setOnComplete(() => form.EnablePaper());
    }

    public void Print(LostItem lostItem)
    {
        Form form = Instantiate(prefabForm, printSpawn.position, transform.rotation).GetComponent<Form>();
        form.SetText("John Doe", lostItem);
        LeanTween.move(form.gameObject, printSpawn.position - printSpawn.forward * tweenDistance, tweenDuration).setOnComplete(() => form.EnablePaper());
    }
}