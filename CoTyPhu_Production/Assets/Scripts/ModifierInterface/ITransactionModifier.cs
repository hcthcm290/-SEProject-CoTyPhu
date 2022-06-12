using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITransactionModifier
{
    bool isActivated(Player player, int baseAmount, bool isBetweenPlayer);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="target"></param>
    /// <param name="baseAmount"></param>
    /// <param name="amount"></param>
    /// <returns>Tuple<player doing transaction, new base amount, new delta></returns>
    System.Tuple<Player, int, int> ModifyTransaction(Player target, int baseAmount, int delta);
}
