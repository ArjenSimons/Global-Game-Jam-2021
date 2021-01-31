using System;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

public class Form : MonoBehaviour
{
    [Header("Scene References")]
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

    [SerializeField]
    private Collider paperCollider = null;

    [Header("Project References")]
    [SerializeField]
    private FormSet formSet = null;

    public LostItem ItemDisplaying { get; private set; }

    [Header("Data files")]
    [SerializeField]
    private TextAsset RandomNamesFile = null;

    private string[] names;

    private void Awake()
    {
        formSet.Add(this);

        names = RandomNamesFile.text.Split(
            new[] { "\r\n", "\r", "\n" },
            StringSplitOptions.None
);
    }

    private void OnDestroy()
    {
        formSet.Remove(this);
    }

    public void SetText(string name, string item, string color, string remarks = "")
    {
        textName.text = names[UnityEngine.Random.Range(0, names.Length)];
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
    }
}