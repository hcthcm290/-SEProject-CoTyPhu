using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StatusGoldReceiveChange : BaseStatus, ITransactionModifier
{
    /// <summary>
    /// this status for gold recveive change with no limit (change come from all sources are count)
    /// </summary>
    public float goldReceiveChange { get; set; }
    public Player targetPlayer;

    //public float GetGoldReceiveChange(float basePrice, float delta)
    //{
    //    delta += goldReceiveChange * basePrice;

    //    return delta;
    //}

    public bool isActivated(Player player, int amount, bool isBetweenPlayer)
    {
        if (player == targetPlayer)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public Tuple<Player, int, int> ModifyTransaction(Player target, int baseAmount, int delta)
    {
        int newDelta = (int)(delta + baseAmount * goldReceiveChange);

        Tuple<Player, int, int> result = new Tuple<Player, int, int>(target, baseAmount, newDelta);

        return result;
    }

    public override bool LoadData()
    {
        return true;
    }

    public override bool StartListen()
    {
        if(targetPlayer != null)
        {
            //targetPlayer.AddStatus(this);
            Bank.Ins.AddReceiveMoneyStatus(this);

            gameObject.AddComponent<ExpiredOnTurn>();
            gameObject.GetComponent<ExpiredOnTurn>().Init(this, 1);
            return true;
        }
        else
        {
            Debug.Log("Must set target player before listening");
            return false;
        }

    }

    public override bool Remove(bool triggerEvent)
    {
        Bank.Ins.RemoveReceiveMoneyStatus(this);
        //targetPlayer.RemoveStatus(this);
        base.Remove(triggerEvent);

        Destroy(this.gameObject);

        return true;
    }

    public override bool ExcuteAction()
    {
        return true;
    }
}
