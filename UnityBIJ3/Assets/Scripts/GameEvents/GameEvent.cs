using System;
using System.Collections.Generic;
using UnityEngine;

public class GameEvent : MonoBehaviour
{
    public static GameEvent Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
        Subscriptions = new Dictionary<string, List<Action<IGameEventBase>>>();
    }

    private Dictionary<string, List<Action<IGameEventBase>>> Subscriptions { get; set; }

    public static void RaiseEvent<T>(T gameEvent)
        where T : IGameEventBase
    {
        var eventKey = typeof(T).FullName;
        if (!Instance.Subscriptions.ContainsKey(eventKey))
        {
            Instance.Subscriptions[eventKey] = new List<Action<IGameEventBase>>();
        }

        var subscriptions = Instance.Subscriptions[eventKey];
        foreach (var subscription in subscriptions)
        {
            try
            {
                subscription(gameEvent);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }
    }

    public static void Subscribe<T>(Action<T> callback)
        where T : IGameEventBase
    {
        var eventKey = typeof(T).FullName;
        if (!Instance.Subscriptions.ContainsKey(eventKey))
        {
            Instance.Subscriptions[eventKey] = new List<Action<IGameEventBase>>();
        }

        var action = new Action<IGameEventBase>(e => callback((T)e));
        Instance.Subscriptions[eventKey].Add(action);
    }
}
