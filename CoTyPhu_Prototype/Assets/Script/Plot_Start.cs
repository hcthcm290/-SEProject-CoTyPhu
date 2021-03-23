using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plot_Start : BasePlot
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void ActivePlotPassByEffect(PlayerControl p)
    {
        //base.ActivePlotPassByEffect(p);
        p.SendMessage("GainGold", 200);
        Debug.Log("Gained 200 Gold");
    }
}
