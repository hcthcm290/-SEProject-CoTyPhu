using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Passive_Birger : BaseStatus
{
    public Player targetPlayer;

    public Passive_Birger()
    {
        _id = 6;
        _name = "Heritage";
        _description = "Start the game with extra 600 GOLD.";
        _isConditional = true;
    }

    public override bool PassiveSetup(Player p)
    {
        targetPlayer = p;
        StartListen();
        return base.PassiveSetup(p);
    }

    public override bool LoadData()
    {
        return true;
    }

    public override bool StartListen()
    {
        if (targetPlayer != null)
        {
            Bank.Ins.SendMoney(targetPlayer, 600, false);
            return true;
        }
        else
        {
            Debug.Log("Must set target player before listening");
            return false;
        }
    }

    public override bool ExcuteAction()
    {
        //negative the status here and then self remove
        return true;
    }
}
