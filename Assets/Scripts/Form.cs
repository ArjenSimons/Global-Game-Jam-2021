using TMPro;
using UnityEngine;

public class Form : MonoBehaviour
{
    [SerializeField]
    private Rigidbody rigidBody = null;

    [SerializeField]
    private TMP_Text textName = null;

    [SerializeField]
    private TMP_Text textItem = null;

    [SerializeField]
    private TMP_Text textColor = null;

    [SerializeField]
    private TMP_Text textRemarks = null;

    private Collider paperCollider;

    public LostItem ItemDisplaying { get; private set; }

    private void Start()
    {
        paperCollider = GetComponent<Collider>();
        paperCollider.enabled = false;
        //TODO: make object not grabbable by player
    }

    public void SetText(string name, string item, string color, string remarks = "")
    {
        textName.text = name;
        textItem.text = item;
        textColor.text = color;
        textRemarks.text = remarks;
    }

    public void SetText(string name, LostItem item)
    {
        SetText(name, item.ItemType.DisplayName, item.ItemColor.DisplayName);
        ItemDisplaying = item;
    }

    public void EnablePaper()
    {
        paperCollider.enabled = true;
        rigidBody.isKinematic = false;
        //TODO: make object grabbable by player
    }
}