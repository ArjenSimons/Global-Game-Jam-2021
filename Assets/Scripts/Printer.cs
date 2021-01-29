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
        Print("Yeehaw");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            Print("typeofTeam");
        }
    }

    public void Print(string text)
    {
        Form form = Instantiate(prefabForm, printSpawn.position, transform.rotation).GetComponent<Form>();
        form.SetText(text);
        LeanTween.move(form.gameObject, printSpawn.position - printSpawn.forward * tweenDistance, tweenDuration).setOnComplete(() => form.EnablePaper());
    }

    public void Print(LostItem lostItem)
    {

    }
}
