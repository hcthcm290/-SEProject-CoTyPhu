using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plot_Fest : BasePlot
{
    public BaseStatusDetail festivalPrefab;
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
        AddStateFestival(p);
    }

    public void AddStateFestival(PlayerControl player)
    {
        BaseStatusDetail ss = Instantiate(festivalPrefab, this.transform);
        GameObject.Find("StatusManager").SendMessage("AddStatusToList", ss);
        ss.gameObject.name = "Lễ hội náo nhiệt";
        ss.description = "Gia tăng 100% tiền thuê của ô đất này.";
        ss.source = player;
    }
}
