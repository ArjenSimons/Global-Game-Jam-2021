using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlowObjectGroup : MonoBehaviour
{
    [SerializeField]
    private GlowObject[] objects;

    private void Awake()
    {
        if (objects == null)
        {
            objects = GetComponentsInChildren<GlowObject>();
        }
    }

    private void Reset()
    {
        objects = GetComponentsInChildren<GlowObject>();
    }

    public void SetColor(Color color)
    {
        foreach (GlowObject glowObject in objects)
        {
            glowObject.GlowColor = color;
        }
    }

    private void OnMouseEnter()
    {
        foreach (GlowObject glowObject in objects)
        {
            glowObject.SetGlow(true);
        }
    }

    private void OnMouseExit()
    {
        foreach (GlowObject glowObject in objects)
        {
            glowObject.SetGlow(false);
        }
    }
}