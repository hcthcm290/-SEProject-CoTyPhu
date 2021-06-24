using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusJeweryDeal : BaseStatus, IPlotEnterListener
{
    public BaseStatus _subStatusPrefab;
    public BaseStatus subStatus;
    public Player targetPlayer;

    public StatusJeweryDeal()
    {
        _id = 5;
        _name = "Jewery Deal";
        _description = "When enter OTHER player's MARKET PLOT gain 200 GOLD, otherwise, lose 100 GOLD.Last TWO ROUND";
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
            gameObject.AddComponent<ExpiredOnRound>();
            gameObject.GetComponent<ExpiredOnRound>().Init(this, 2);
            //add a code to let this status listen to when player is affect by an status
            for (int i = 0; i < 32; i++)
            {
                Plot.plotDictionary[(PLOT)i].SubcribePlotEnter(this);
            }
            return true;
        }
        else
        {
            Debug.Log("Must set target player before listening");
            return false;
        }
    }

    public void OnPlotEnter(Player player, Plot plot)
    {
        Debug.Log("Receive On Plot Pass By");
        if (player == targetPlayer)
        {
            if (plot is PlotConstructionMarket)
            {
                PlotConstructionMarket p = (PlotConstructionMarket)plot;
                if(p.Owner != player)
                {
                    //Nhan 200 gold o day
                    Bank.Ins.SendMoney(targetPlayer, 200);
                    return;
                }
            }
            //Mat 100 gold o day
            Bank.Ins.TakeMoney(targetPlayer, 100);
            
        }
    }

    public override bool Remove(bool triggerEvent)
    {
        for (int i = 0; i < 32; i++)
        {
            Plot.plotDictionary[(PLOT)i].UnsubcribePlotEnter(this);
        }
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
