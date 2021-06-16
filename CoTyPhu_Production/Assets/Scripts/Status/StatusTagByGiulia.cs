using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusTagByGiulia : BaseStatus
{
    public Player targetPlayer;
    public Player Owner;
    public int currentMana;

    public StatusTagByGiulia()
    {
        _id = 5;
        _name = "Mana thief (target)";
        _description = "When you gain mana(s), owner of this tag gain 1 mana.";
        _isConditional = true;
    }

    public override bool LoadData()
    {
        return true;
    }

    public override bool StartListen()
    {
        if (targetPlayer != null)
        {
            targetPlayer.AddStatus(this);
            targetPlayer.ManaChange += ManaThief;

            currentMana = targetPlayer.GetMana();
            return true;
        }
        else
        {
            Debug.Log("Must set target player before listening");
            return false;
        }
    }

    public void ManaThief()
    {
        if (targetPlayer.GetMana() > currentMana)
        {
            Owner.ChangeMana(1);
        }
        currentMana = targetPlayer.GetMana();
    }

    public override bool Remove(bool triggerEvent)
    {
        targetPlayer.ManaChange -= ManaThief;
        targetPlayer.RemoveStatus(this);
        base.Remove(triggerEvent);

        return true;
    }

    public override bool ExcuteAction()
    {
        //negative the status here and then self remove
        return true;
    }
}
