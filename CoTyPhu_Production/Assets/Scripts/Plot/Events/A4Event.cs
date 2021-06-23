using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Riots
/// All players lose 200$
/// </summary>
public class A4Event : EventAction
{
    public override void PerformAction()
    {
        target.AddItem(Object.Instantiate(PrefabContainer.Ins.mosesStaff));
        if (target.MinePlayer && TurnDirector.Ins.IdPhase == Phase.Stop)
            TurnDirector.Ins.EndOfPhase();
    }
}
