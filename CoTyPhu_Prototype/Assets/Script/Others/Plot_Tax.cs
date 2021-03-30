using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plot_Tax : BasePlot
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void ActivePlotEffect(PlayerControl p)
    {
        base.ActivePlotEffect(p);
        p.SendMessage("LoseGold", 200);
        Debug.Log("Lost 200 Gold");
    }
}
