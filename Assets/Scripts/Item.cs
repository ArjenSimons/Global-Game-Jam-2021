using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField]
    private List<MeshRenderer> renderersToColor = null;

    [SerializeField]
    private ItemSet items = null;

    [Space]
    public bool CanTriggerDropOffButton = false;

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
            renderer.materials[renderer.materials.Length - 1].color = color;
        }
    }
}