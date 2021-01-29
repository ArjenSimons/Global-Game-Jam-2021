using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemColorSetter : MonoBehaviour
{
    [SerializeField]
    private List<MeshRenderer> renderersToColor = null;

    public void SetColor(Color color)
    {
        foreach (MeshRenderer renderer in renderersToColor)
        {
            renderer.material.color = color;
        }
    }
}
