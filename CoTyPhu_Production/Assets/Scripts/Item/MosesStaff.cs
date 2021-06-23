using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MosesStaff : BaseItem
{
    public override bool LoadData()
    {
        return true;
    }

    public override bool StartListen()
    {
        return false;
    }

    public override bool Activate(string activeCase)
    {

        Owner.RemoveItem(this);
        Destroy(gameObject);
        // player active animation

        // Add item back to Item pool
        ItemManager.Ins.AddItemToPool(this);

        ((PlotPrison)Plot.plotDictionary[PLOT.PRISON]).Release(Owner);
        StopPhaseUI.Ins.Deactive(PhaseScreens.FreeCardUI);

        return base.Activate(activeCase);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (((PlotPrison)Plot.plotDictionary[PLOT.PRISON]).PlayerImprisonDuration(Owner) > 0)
            CanActivate = true;
        else
            CanActivate = false;
    }
}
