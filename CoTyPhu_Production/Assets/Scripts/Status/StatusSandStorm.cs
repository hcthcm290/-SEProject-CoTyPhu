using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusSandStorm : BaseStatus, IHirePriceChange
{
    public PlotConstruction targetPlot;
    public float hirePriceChange { get; set; }
    public StatusSandStorm()
    {
        _id = 1;
        _name = "Bão cát";
        _description = "Một trận bão cát quét qua gây cản trở một phần giao thương.\nGiảm 50% giá thuê ô đất này.";
        _isConditional = false;
        hirePriceChange = -0.5f; //-50%
    }

    //Effect Aplly here
    public float GethirePriceChange(float basePrice, float delta)
    {
        return delta + basePrice * hirePriceChange;
    }

    public override bool LoadData()
    {
        try
        {
            //ExpiredOnTurn e = new ExpiredOnTurn(this, 5);
            return true;
        }
        catch (Exception e)
        {
            return false;
        }
    }

    public override bool StartListen()
    {
        if (targetPlot != null)
        {
            gameObject.AddComponent<ExpiredOnTurn>();
            gameObject.GetComponent<ExpiredOnTurn>().Init(this, 5);
            //add a code to let this status listen to when player is affect by an status
            targetPlot.AddStatus(this);
            return true;
        }
        else
        {
            Debug.Log("Must set target plot before listening");
            return false;
        }
    }

    public override bool ExcuteAction()
    {
        return true;
    }

    public override bool Remove(bool triggerEvent)
    {
        targetPlot.RemoveStatus(this);
        base.Remove(triggerEvent);
        Destroy(this.gameObject);

        return true;
    }
}
