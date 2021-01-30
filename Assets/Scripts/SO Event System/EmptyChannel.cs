using UnityEngine;
using System;

[CreateAssetMenu(menuName = "SO Event Channels/Empty")]
public class EmptyChannel : ScriptableObject
{
    public Action OnEventRaised;

    public void RaiseEvent()
    {
        if (OnEventRaised != null)
        {
            OnEventRaised();
        }
        else
        {
            Debug.LogWarning("an event for a dropped lost item was raised, but no one was listening ::" +
                "make sure the printer is listening");
        }
    }
}
