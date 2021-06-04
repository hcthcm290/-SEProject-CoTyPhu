using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SunnaryItem : BaseItem
{
    protected static int SunnaryActivationCount = 0;
    public override bool Activate(string activeCase)
    {
        SunnaryActivationCount++;

        return base.Activate(activeCase);
    }
}
