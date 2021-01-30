using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField]
    private List<MeshRenderer> renderersToColor = null;

    [SerializeField]
    private ItemSet items = null;

    public LostItem LostItem;

    private void Awake()
    {
        items.Add(this);
    }

    private void OnDestroy()
    {
        items.Remove(this);
    }

    public void SetColor(Color color)
    {
        foreach (MeshRenderer renderer in renderersToColor)
        {
            renderer.material.color = color;
        }
    }
}