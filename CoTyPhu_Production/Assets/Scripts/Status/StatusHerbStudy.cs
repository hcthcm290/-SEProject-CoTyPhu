using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusHerbStudy : BaseStatus, IOtherActivate
{
    public Player targetPlayer;

    public StatusHerbStudy()
    {
        _id = 2;
        _name = "Herb protection";
        _description = "Remove STATUS NEGATIVE on you ONE TIME. Last for ONE ROUND.";
        _isConditional = true;
    }

    public override bool LoadData()
    {
        try
        {
            return true;
        }
        catch (Exception e)
        {
            return false;
        }
    }

    public override bool StartListen()
    {
        if (targetPlayer != null)
        {
            //targetPlayer.AddStatus(this);
            gameObject.AddComponent<ExpiredOnRound>();
            gameObject.GetComponent<ExpiredOnRound>().Init(this, 1);
            //add a code to let this status listen to when player is affect by an status
            targetPlayer.StatusAdding += Activate;
            return true;
        }
        else
        {
            Debug.Log("Must set target player before listening");
            return false;
        }
    }

    public void Activate(BaseStatus status)
    {
        if (status.Type == "Negative")
        {
            targetPlayer.RemoveStatus(status);
            //targetPlayer.RemoveStatus(this);
        }
    }

    public override bool Remove(bool triggerEvent)
    {
        targetPlayer.RemoveStatus(this);
        base.Remove(triggerEvent);
        Destroy(this.gameObject);

        return true;
    }

    public override bool ExcuteAction()
    {
        //negative the status here and then self remove
        return true;
    }
}
