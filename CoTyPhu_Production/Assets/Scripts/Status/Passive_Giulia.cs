using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Passive_Giulia : BaseStatus, IOtherActivate
{
    public Player targetPlayer;

    public Passive_Giulia()
    {
        _id = 7;
        _name = "Mana thief";
        _description = "TAG a player that affect by your SKILL. GAIN ONE MANA each time TAG player gain mana(s). You can only TAG each player once.";
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
        //be call by skill
        return true;
    }
}
