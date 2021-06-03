using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlotRebuyUI : PlotBuyUI
{
    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();
        _txtPrice.text = "$" + Plot?.PurchasePrice.ToString();
    }

    public override PhaseScreens GetScreenType()
    {
        return PhaseScreens.PlotRebuyUI;
    }
}
