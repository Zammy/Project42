using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public interface AnimEventReceiver
{
    void ReceiveEvent(string name);
}

public class AnimEventRelay : MonoBehaviour
{
    Dictionary<string, AnimEventReceiver> eventReceivers = new Dictionary<string, AnimEventReceiver>();
    
    public void SubscribeToEvent(string eventName, AnimEventReceiver receiver)
    {
        eventReceivers.Add(eventName, receiver);
    }

    public void UnsubscribeToAll(AnimEventReceiver receiver)
    {
        foreach (var item in eventReceivers.Where(kvp => kvp.Value == receiver).ToList())
        {
            eventReceivers.Remove(item.Key);
        }
    }

	public void ReceiveEvent(string eventName)
    {
        AnimEventReceiver receiver;
        if (eventReceivers.TryGetValue(eventName, out receiver))
        {
            receiver.ReceiveEvent(eventName);
        }
    }
}
