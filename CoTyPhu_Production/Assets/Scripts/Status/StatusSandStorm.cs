using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusSandStorm : BaseStatus, IHirePriceChange
{
    public float hirePriceChange { get; set; }
    public StatusSandStorm()
    {
        _id = 1;
        _name = "Bão cát";
        _description = "Một trận bão cát quét qua gây cản trở một phần giao thương.\nGiảm 50% giá thuê ô đất này.";
        _isConditional = false;
        hirePriceChange = -0.5f; //-50%
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
        return true;
    }

    public override bool ExcuteAction()
    {
        return true;
    }

    //Effect Aplly here
    public float GethirePriceChange(float basePrice, float delta)
    {
        return delta + basePrice * hirePriceChange;
    }
}
