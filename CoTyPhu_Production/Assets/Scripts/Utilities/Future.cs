using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FutureTask<T>
{
    private Future<T> future;
    private Action<T> futureThen;

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

    public Future(ref Action<T> FutureTaskFunction)
    {
        FutureTaskFunction = Then;
    }

    private void Then(T t)
    {
        _callbackFunction(t);
    }

    public void then(Action<T> callbackFunction)
    {
        _callbackFunction = callbackFunction;
    }
}
