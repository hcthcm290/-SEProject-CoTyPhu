using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial interface IAction
{
    void PerformAction();
}
public interface ICompletableAction : IAction
{
    IAction OnActionComplete { get; set; }

    void PerformOnComplete();
}
public class ActionList : ICompletableAction
{
    Queue<IAction> Actions = new Queue<IAction>();
    private IAction _onComplete;
    public IAction OnActionComplete { get => _onComplete; set { _onComplete = value; } }

    public void PerformAction()
    {
        if (Actions.Count > 0)
            Actions.Dequeue().PerformAction();
        else
            PerformOnComplete();
    }
    protected void PerformNextAction()
    {
        if (Actions.Count > 0)
            Actions.Dequeue().PerformAction();
        else
            PerformOnComplete();
    }
    public void AddNonBlockAction(IAction action)
    {
        Actions.Enqueue(new LambdaAction(action, PerformNextAction));
    }
    public void AddBlockingAction(ICompletableAction action)
    {
        action.OnActionComplete = new LambdaAction(action.OnActionComplete, PerformNextAction);
        Actions.Enqueue(action);
    } 

    public void PerformOnComplete()
    {
        OnActionComplete?.PerformAction();
        OnActionComplete = null;
    }

    public void AddOnActionComplete(Action action)
    {
        if (action != null)
            OnActionComplete = new LambdaAction(OnActionComplete, action);
    }
}

/// <summary>
/// wraps an Action around a preAction and / or a postAction,
/// both of which are Lambdas
/// </summary>
public class LambdaAction : IAction
{
    public System.Action preAction;
    public IAction wrappedAction;
    public System.Action postAction;
    public LambdaAction(System.Action preAction, 
        IAction wrappedAction = null, System.Action postAction = null) 
    { 
        this.preAction = preAction;
        this.wrappedAction = wrappedAction;
        this.postAction = postAction;
    }
    public LambdaAction(IAction wrappedAction, System.Action postAction)
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

public class LambdaCompletableAction : LambdaAction, ICompletableAction
{
    private IAction _onComplete;
    public IAction OnActionComplete { get => _onComplete; set { _onComplete = value; } }

    public void PerformOnComplete()
    {
        OnActionComplete?.PerformAction();
    }

    public LambdaCompletableAction(System.Action preAction,
        IAction wrappedAction = null, System.Action postAction = null)
        : base(preAction, wrappedAction, postAction) { }
    public LambdaCompletableAction(IAction wrappedAction, System.Action postAction)
        : base(wrappedAction, postAction) { }
}

public static partial class Extensions
{
    public static IAction Add (this IAction action, Action other)
    {
        return new LambdaAction(action, other);
    }
    public static IAction Add(this IAction action, IAction other)
    {
        return new LambdaAction(action, () => other?.PerformAction());
    }
}