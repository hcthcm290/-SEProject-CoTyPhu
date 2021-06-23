using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Bandit
/// You lose 1000$
/// </summary>
public class A3Event : EventAction, ITransaction
{
    public int moneyAmount = 100;

    public int MoneyAmount
    {
        get => moneyAmount;
    }

    public object Source
    {
        get => target;
    }
    public object Destination
    {
        get => Bank.Ins;
    }

    public override void PerformAction()
    {
        Bank.Ins.TakeMoney(target, moneyAmount);
        if (target.MinePlayer && TurnDirector.Ins.IdPhase == Phase.Stop)
            TurnDirector.Ins.EndOfPhase();
    }
}
