using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlowObjectGroup : MonoBehaviour
{
    [SerializeField]
    private SharedPlayerData playerData = null;

    [SerializeField]
    private GlowObject[] objects;

    private bool isGlowing;
    private bool isOver;

    private void Awake()
    {
        if (objects == null)
        {
            FetchObjects();
        }
    }

    private void Reset()
    {
        FetchObjects();
    }

    private void FetchObjects()
    {
        objects = GetComponentsInChildren<GlowObject>();

        foreach (GlowObject glowObject in objects)
        {
            glowObject.PartOfGroup = true;
        }
    }

    public void SetColor(Color color)
    {
        foreach (GlowObject glowObject in objects)
        {
            glowObject.GlowColor = color;
        }
    }

    private void SetGlow(bool value)
    {
        foreach (GlowObject glowObject in objects)
        {
            glowObject.SetGlow(value);
        }

        isGlowing = value;
    }

    private void Update()
    {
        if (!isOver)
        {
            return;
        }

        if (isGlowing && !playerData.controller.IsInGrabRange(transform.position))
        {
            //a glowing object out of range needs to unglow
            SetGlow(false);
        }
        else if (!isGlowing && playerData.controller.IsInGrabRange(transform.position))
        {
            //an unglowing object that comes into range needs to start glowing
            SetGlow(true);
        }
    }

    private void OnMouseEnter()
    {
        if (playerData.controller.IsInGrabRange(transform.position))
        {
            SetGlow(true);
        }

        isOver = true;
    }

    private void OnMouseExit()
    {
        SetGlow(false);

        isOver = false;
    }
}