using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FutureTask<T>
{
    private Future<T> future;
    private Action<T> futureThen;

    public static Future<bool> Delay(float duration)
    {
        var delayObject = new GameObject();
        var delayScript = delayObject.AddComponent<DelayObject>();
        return delayScript.Init(duration);
    }

    public FutureTask()
    {
        future = new Future<T>(ref futureThen);
    }

    public Future<T> GetFuture()
    {
        return future;
    }

    public void Complete(T t)
    {
        futureThen(t);
    }
}

public class Future<T>
{
    private Action<T> _callbackFunction;
    private bool completed = false;
    private T _t;

    public Future(ref Action<T> FutureTaskFunction)
    {
        FutureTaskFunction = Then;
    }

    private void Then(T t)
    {
        if (_callbackFunction != null && !completed)
        {
            _callbackFunction(t);
        }
        else
        {
            _t = t;
        }

        completed = true;
    }

    public void then(Action<T> callbackFunction)
    {
        if(_callbackFunction == null && completed)
        {
            callbackFunction(_t);
        }
        else
        {
            _callbackFunction = callbackFunction;
        }

    }
}


