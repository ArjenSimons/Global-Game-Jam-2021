using TMPro;
using UnityEngine;

public class Computer : MonoBehaviour
{
    [SerializeField]
    private TMP_Text textTime = null;

    [SerializeField]
    private TMP_Text textForms = null;

    public void SetFormsText(int amountCompleted)
    {
        textForms.text = amountCompleted + " forms completed";
    }
}
