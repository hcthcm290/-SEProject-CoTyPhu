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
    protected int _wageMoney;


    //  Initialization --------------------------------
    public PlotStart(PLOT id, string name, string description, int wageMoney) : base(id, name, description)
    {
        this._wageMoney = wageMoney;
    }


    //  Methods ---------------------------------------
    public override void ActionOnPass(dynamic obj)
    {
        this.ActionOnPass(obj);
        //TODO: Give money when pass this plot
    }


    //  Event Handlers --------------------------------
    
}
