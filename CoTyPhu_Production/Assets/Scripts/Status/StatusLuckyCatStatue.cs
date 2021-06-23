using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusLuckyCatStatue : BaseStatus, ITransactionModifier
{
    #region Properties
    public Player Owner;
    [SerializeField] LuckyCatStatue item;
    [SerializeField] float transactionModifyFactor;
    #endregion

    #region Base class override
    public override bool ExcuteAction()
    {
        Remove(true);
        return true;
    }

    public override bool Remove(bool triggerEvent)
    {
        Bank.Ins.RemoveReceiveMoneyStatus(this);
        item.Remove(triggerEvent);
        base.Remove(triggerEvent);
        return true;
    }

    public bool isActivated(Player player, int baseAmount, bool isBetweenPlayer)
    {
        if(player == Owner && isBetweenPlayer)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public override bool LoadData()
    {
        return true;
    }

    public Tuple<Player, int, int> ModifyTransaction(Player target, int baseAmount, int delta)
    {
        int newDelta = (int)(delta + baseAmount * transactionModifyFactor);

        Tuple<Player, int, int> result = new Tuple<Player, int, int>(target, baseAmount, newDelta);

        return result;
    }

    public override bool StartListen()
    {
        Bank.Ins.AddReceiveMoneyStatus(this);
        return true;
    }
    #endregion
}
