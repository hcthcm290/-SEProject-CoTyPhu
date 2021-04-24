using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial interface Action
{
    public void PerformAction();
}
/// <summary>
/// wraps an Action around a preAction and / or a postAction,
/// both of which are Lambdas
/// </summary>
public class LambdaAction : Action
{
    System.Action preAction;
    Action wrappedAction;
    System.Action postAction;
    public LambdaAction(System.Action preAction, 
        Action wrappedAction = null, System.Action postAction = null) 
    { 
        this.preAction = preAction;
        this.wrappedAction = wrappedAction;
        this.postAction = postAction;
    }
    public LambdaAction(Action wrappedAction, System.Action postAction)
    {
        this.preAction = null;
        this.wrappedAction = wrappedAction;
        this.postAction = postAction;
    }
    public void PerformAction()
    {
        preAction?.Invoke();
        wrappedAction?.PerformAction();
        postAction?.Invoke();
    }
}