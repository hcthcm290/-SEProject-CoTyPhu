using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item001_WandererDice : BaseItem
{
    public Item001_WandererDice()
    {
        LoadData();
    }

    public void Start()
    {

    }

    public override bool LoadData()
    {
        Set(
            id: 1,
            name: "Wanderer Dice",
            price: 300,
            description: "A dice that is enhanced with magic that make the user does not feel tired even he/she has traveled a thoundsand-miles journey",
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
