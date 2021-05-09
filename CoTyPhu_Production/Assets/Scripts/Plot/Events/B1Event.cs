using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Riots
/// All players lose 200$
/// </summary>
public class B1Event : PlayerBasedAction, ITransaction
{
    public int moneyAmount = 200;

    public int MoneyAmount
    {
        get => moneyAmount;
    }

    public object Source
    {
        get => Bank.Ins.AllMoneyPlayers.Keys.ToList();
    }
    public object Destination
    {
        get => Bank.Ins;
    }

    public override void PerformAction()
    {
        foreach(Player player in (Source as List<Player>))
            Bank.Ins.TakeMoney(player, moneyAmount);

        TurnDirector.Ins.EndOfPhase();
    }
}
