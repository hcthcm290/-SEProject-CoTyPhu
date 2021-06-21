using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// PLOT.START (0) GIVE MONEY TO PLAYER WHEN PASS, AS WELL AS OPEN THE SHOP
/// </summary>
public class PlotStart : Plot
{
    //  Events ----------------------------------------


    //  Properties ------------------------------------
    public int WageMoney { get => _wageMoney; }


    //  Fields ----------------------------------------
    [SerializeField] protected int _wageMoney;


    //  Initialization --------------------------------
    public PlotStart(PLOT id, string name, string description, int wageMoney) : base(id, name, description)
    {
        this._wageMoney = wageMoney;
    }


    //  Methods ---------------------------------------
    public override IAction ActionOnPass(Player obj)
    {
        return new LambdaAction(() =>
        {
            //Give money when pass this plot
            Bank.Ins.SendMoney(obj, _wageMoney);

            // Notify turn director it's pass the start plot
            if (obj.MinePlayer)
            {
                TurnDirector.Ins.NotifyPassPlotStart(obj);
            }
            NotifyPlotPassBy(obj);
        }, base.ActionOnPass(obj));
    }


    //  Event Handlers --------------------------------

}
