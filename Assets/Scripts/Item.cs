using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField]
    private List<MeshRenderer> renderersToColor = null;

    public LostItem LostItem;

    public void SetColor(Color color)
    {
        foreach (MeshRenderer renderer in renderersToColor)
        {
            renderer.material.color = color;
        }
    }
}
