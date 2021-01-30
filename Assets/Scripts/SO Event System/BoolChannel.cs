using UnityEngine;
using System;

[CreateAssetMenu(menuName = "SO Event Channels/Boolean")]
public class BoolChannel : ScriptableObject
{
    public Action<bool> OnEventRaised;

    public void RaiseEvent(bool value)
    {
        if (OnEventRaised != null)
        {
            OnEventRaised(value);
        }
        else
        {
            Debug.LogWarning("an event for a dropped lost item was raised, but no one was listening ::" +
                "make sure the printer is listening");
        }
    }
}