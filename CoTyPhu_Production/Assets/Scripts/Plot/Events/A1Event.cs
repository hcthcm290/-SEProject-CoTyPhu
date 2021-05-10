using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Arrested!
/// Move to PRISON immediately
/// </summary>
public class A1Event : EventAction
{
    public override void PerformAction()
    {
        target.MoveTo(PLOT.PRISON);
        target.moveComponent.ListenTargetReached(new LambdaAction(() => 
        {
            target.StartPhase(Phase.Stop);
        }));
    }
}
