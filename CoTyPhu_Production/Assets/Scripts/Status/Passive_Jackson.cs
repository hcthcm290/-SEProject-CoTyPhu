using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Passive_Jackson : BaseStatus, IPlotPassByListener
{
    public Player targetPlayer;
    public StatusGoldReceiveChange receive;
    public StatusGoldReceiveChange save;

    public Passive_Jackson()
    {
        _id = 4;
        _name = "Extra Tip";
        _description = "Get 5% more gold when passing by PLOT START.";
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
            //add a code to let this status listen to when player is affect by an status

            Plot.plotDictionary[PLOT.H2].SubcribePlotPassByListener(this);
            Plot.plotDictionary[PLOT.A1].SubcribePlotPassByListener(this);
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

    public void OnPlotPassBy(Player player, Plot plot)
    {
        Debug.Log("Receive On Plot Pass by");
        if (player == targetPlayer)
        {
            if (plot.Id == PLOT.H2)
            {
                var newStatus = Instantiate(receive, targetPlayer.transform);
                newStatus.goldReceiveChange = 0.1f;
                newStatus.targetPlayer = targetPlayer;
                newStatus.StartListen();
                save = newStatus;
            }

            if (plot.Id == PLOT.A1)
            {
                if (save)
                {
                    save.Remove(true);
                    save = null;
                }
            }
        }
    }
}
