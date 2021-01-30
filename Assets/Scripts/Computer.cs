using TMPro;
using UnityEngine;

public class Computer : MonoBehaviour
{
    [Header("Prefab references")]
    [SerializeField]
    private TMP_Text textTime = null;

    [SerializeField]
    private TMP_Text textCorrectForms = null;

    [SerializeField]
    private TMP_Text textIncorrectForms = null;

    [Header("Scene references")]
    [SerializeField]
    private ScoresManager scoreManager = null;

    [Header("Channel Broadcasting on")]
    [SerializeField]
    private EmptyChannel scoresUpdatedChannel = null;

    private void Start()
    {
        SetTexts();
        scoresUpdatedChannel.OnEventRaised += SetTexts;
    }

    private void SetTexts()
    {
        textCorrectForms.text = scoreManager.CorrectForms + " forms completed";
        textIncorrectForms.text = scoreManager.IncorrectForms + " mistakes";

    }
}
