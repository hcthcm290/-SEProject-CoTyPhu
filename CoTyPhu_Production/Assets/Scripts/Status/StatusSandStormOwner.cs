using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusSandStormOwner : BaseStatus, IPlotPassByListener
{
    public Player targetPlayer;
    public StatusSandStorm storm;

    public StatusSandStormOwner()
    {
        _id = 10;
        _name = "Gieo rắc bão cát";
        _description = "Đặt hiệu ứng BÃO CÁT lên những ô bạn đi ngang qua.";
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
            gameObject.AddComponent<ExpiredOnTurn>();
            gameObject.GetComponent<ExpiredOnTurn>().Init(this, 1);
            //add a code to let this status listen to when player is affect by an status
            for (int i = 0; i < 32; i++)
            {
                Plot.plotDictionary[(PLOT) i].SubcribePlotPassByListener(this);
            }
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
        Debug.Log("Receive On Plot Pass By");
        if (player == targetPlayer)
        {
            if (plot is PlotConstructionMarket)
            {
                var newStatus = Instantiate(storm, plot.transform);
                newStatus.targetPlot = (PlotConstruction)plot;
                newStatus.StartListen();
                //((PlotConstruction)plot).AddStatus(hire);
            }
        }
    }

    public override bool Remove(bool triggerEvent)
    {
        targetPlayer.RemoveStatus(this);
        for (int i = 0; i < 32; i++)
        {
            Plot.plotDictionary[(PLOT)i].UnsubcribePlotPassByListner(this);
        }
        base.Remove(triggerEvent);
        Destroy(this.gameObject);

        return true;
    }
}
