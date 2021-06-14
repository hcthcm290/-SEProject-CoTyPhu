using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plot_Prison : BasePlot
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
        Debug.Log("Jailed");
        p.state_jail = 0;
    }
}
