using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item003_IceDice : BaseItem
{
    public Item003_IceDice()
    {
        LoadData();
    }

    public void Start()
    {

    }

    public override bool LoadData()
    {
        Set(
            id: 3,
            name: "Ice Dice",
            price: 1000,
            description: "When this item is activated, all Market Plots hire price is decrease by 200% its base price (not below 0) until the user next round.",
            type: "Active"
            );
        return true;
    }

    public override bool StartListen()
    {
        return true;
    }

    public override bool Activate(string activeCase)
    {
        return true;
    }
}
