using UnityEngine;

public class Printer : MonoBehaviour
{
    [SerializeField]
    private GameObject prefabForm = null;

    [SerializeField]
    private Transform printSpawn = null;

    [SerializeField]
    private float tweenDistance = 0.38f;

    [SerializeField]
    private float tweenDuration = 3f;

    private void Start()
    {
        Print("Test", "Test print");
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
        form.SetText("John Doe", lostItem.ItemType.DisplayName, lostItem.ItemColor.DisplayName);
        LeanTween.move(form.gameObject, printSpawn.position - printSpawn.forward * tweenDistance, tweenDuration).setOnComplete(() => form.EnablePaper());
    }
}
