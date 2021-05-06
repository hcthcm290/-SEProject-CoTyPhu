using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlotEvent : Plot
{
    public PlotEvent(PLOT id, string name, string description) : base(id, name, description)
    {

    }

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
    }
    public override IAction ActionOnEnter(Player obj)
    {
        // TODO
        return EventListManager.GetInstance().GetAction(obj);
    }
}

public abstract class PlayerBasedAction : IAction
{
    public Player target;
    public abstract void PerformAction();
}

