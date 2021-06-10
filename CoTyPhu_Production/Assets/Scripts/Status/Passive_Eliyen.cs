using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Passive_Eliyen : BaseStatus, IOtherActivate, IPlotEnterListener
{
    public Player targetPlayer;
    public StatusHirePriceChange hire;

    public Passive_Eliyen()
    {
        _id = 3;
        _name = "Thân thiện";
        _description = "Giảm 10% giá MUA và THUÊ đất thuộc tộc ELF";
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

            Plot.plotDictionary[PLOT.F1].SubcribePlotEnter(this);
            Plot.plotDictionary[PLOT.F2].SubcribePlotEnter(this);
            Plot.plotDictionary[PLOT.F3].SubcribePlotEnter(this);

            Plot.plotDictionary[PLOT.G1].SubcribePlotEnter(this);
            Plot.plotDictionary[PLOT.G2].SubcribePlotEnter(this);
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

    public void OnPlotEnter(Player player, Plot plot)
    {
        Debug.Log("Receive On Plot Enter by");
        if (player == targetPlayer)
        {
            if(plot is PlotConstructionMarket)
            {
                var newStatus = Instantiate(hire, plot.transform);
                newStatus.hirePriceChange = -0.1f;
                newStatus.targetPlot = (PlotConstruction)plot;
                newStatus.StartListen();
                newStatus.gameObject.AddComponent<ExpiredOnTurn>();
                newStatus.gameObject.GetComponent<ExpiredOnTurn>().Init(newStatus, 1);
                //((PlotConstruction)plot).AddStatus(hire);
            }
        }
    }
}
