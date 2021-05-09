using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Lottery
/// You gain 1000$
/// </summary>
class A2Event : PlayerBasedAction, ITransaction
{
    public int moneyAmount = 1000;

    public int MoneyAmount 
    { 
        get => moneyAmount;
    }

    public object Source
    {
        get => Bank.Ins;
    }
    public object Destination 
    { 
        get => target; 
    }

    public override void PerformAction()
    {
        Bank.Ins.SendMoney(target, moneyAmount);
        TurnDirector.Ins.EndOfPhase();
    }
}
