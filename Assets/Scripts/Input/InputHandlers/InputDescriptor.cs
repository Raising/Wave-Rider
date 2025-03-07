using System;
using UnityEngine;
using UnityEngine.Events;

public struct InputDescriptor
{
    public string param;
    public int filterParam;
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


    public InputDescriptor(Type handler, string param = "", int filterParam = 0, UnityAction<object> callback = null, UnityAction<object> failCallback = null)
    {
        this.filterParam = filterParam;
        this.param = param;
        this.handler = handler;
        this.callback = callback;
        this.failCallback = failCallback;
    }
    public InputDescriptor(Type handler, int param = 0, int filterParam = 0, UnityAction<object> callback = null, UnityAction<object> failCallback = null)
    {
        this.filterParam = filterParam;
        this.param = param.ToString();
        this.handler = handler;
        this.callback = callback;
        this.failCallback = failCallback;
    }
}