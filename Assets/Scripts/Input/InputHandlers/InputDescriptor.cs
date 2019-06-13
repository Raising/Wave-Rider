using System;
using UnityEngine;
using UnityEngine.Events;

public struct InputDescriptor
{
    public string param;

    public Type handler;
    public UnityAction<object> callback;
    public UnityAction<object> failCallback;
    
    public string Name
    {
        get
        {
            return handler.Name + ":" + param;
        }
    }


    public InputDescriptor(Type handler, string param = "", UnityAction<object> callback = null, UnityAction<object> failCallback = null)
    {
        this.param = param;
        this.handler = handler;
        this.callback = callback;
        this.failCallback = failCallback;
    }
    public InputDescriptor(Type handler, int param = 0, UnityAction<object> callback = null, UnityAction<object> failCallback = null)
    {
        this.param = param.ToString();
        this.handler = handler;
        this.callback = callback;
        this.failCallback = failCallback;
    }
}