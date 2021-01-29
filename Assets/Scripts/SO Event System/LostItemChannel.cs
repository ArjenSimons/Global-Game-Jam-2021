using UnityEngine;
using System;

[CreateAssetMenu(menuName = "SO Event Channels/Lost Item")]
public class LostItemChannel : ScriptableObject
{
    public Action<LostItem> OnEventRaised;

    public void RaiseEvent(LostItem item)
    {
        if (OnEventRaised != null)
        {
            OnEventRaised(item);
        }
        else
        {
            Debug.LogWarning("an event for a dropped lost item was raised, but no one was listening ::" +
                "make sure the printer is listening");
        }
    }
}