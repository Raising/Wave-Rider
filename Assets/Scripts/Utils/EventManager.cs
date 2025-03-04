using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;

[System.Serializable]
public class DynamicEvent : UnityEvent<object>
{

}

public delegate bool VerificableAction();


public class EventManager : MonoBehaviour
{

    private Dictionary<string, DynamicEvent> eventDictionary;


    private static EventManager eventManager;

    public static EventManager Instance
    {
        get
        {
            if (!eventManager)
            {
                eventManager = FindObjectOfType(typeof(EventManager)) as EventManager;

                if (!eventManager)
                {
                    Debug.LogError("There needs to be one active EventManger script on a GameObject in your scene.");
                }
                else
                {
                    eventManager.Init();
                }
            }

            return eventManager;
        }
    }


    void Init()
    {
        if (eventDictionary == null)
        {
            
            eventDictionary = new Dictionary<string, DynamicEvent>();
        }
    }
    /***
     * sTART lISTENING FOR AN EVEN
     * 
     * @returns an action that remove the listening
     */
    public static VerificableAction StartListening(string eventName, UnityAction<object> action)
    {
        DynamicEvent thisEvent = null;
        if (Instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.AddListener(action);
        }
        else
        {
            thisEvent = new DynamicEvent();
            thisEvent.AddListener(action);
            Instance.eventDictionary.Add(eventName, thisEvent);
        }

        return () => StopListening(eventName, action);
    }

    public static bool StopListening(string eventName, UnityAction<object> action)
    {
        if (eventManager == null) return false;
        DynamicEvent thisEvent = null;
        if (Instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.RemoveListener(action);
            return true;
        }
        return false;
    }

    public static void TriggerEvent(string eventName, object parameters)
    {
        DynamicEvent thisEvent = null;

        if (Instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            Debug.Log(eventName);
            thisEvent.Invoke(parameters);
        }
    }

    public static void TriggerEvent(string eventName)
    {
        object parameters = new object();
        TriggerEvent( eventName,  parameters);
    }
}