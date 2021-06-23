using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Riots
/// All players lose 200$
/// </summary>
public class B1Event : EventAction, ITransaction
{
    public int moneyAmount = 50;

    public int MoneyAmount
    {
        get => moneyAmount;
    }

    public object Source
    {
        get => Bank.Ins.AllMoneyPlayers.Keys.ToList().Where(item => !item.HasLost);
    }
    public object Destination
    {
        get => Bank.Ins;
    }

    public override void PerformAction()
    {
        IEnumerable<Player> players = Source as IEnumerable<Player>;
        if (players != null)
            foreach (Player player in (Source as IEnumerable<Player>))
                Bank.Ins.TakeMoney(player, moneyAmount);

        if (target.MinePlayer && TurnDirector.Ins.IdPhase == Phase.Stop)
            TurnDirector.Ins.EndOfPhase();
    }
}
