using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SunnaryWings : BaseItem, IPlotChooserAction, ICompletableAction
{
    bool canActivate = true;

    #region Base class override
    //public override bool CanActivate { get => canActivate; set => canActivate = value; }

    public override bool LoadData()
    {
        Set(
            id: 9,
            name: "Wings Of Sunnari",
            price: 700,
            description: "Immediately move to a plot that's not a BUILDING or TRAVEL plot.",
            type: "Active"
            );
        /*
        var image = GetComponent<Image>();
        if (image != null)
            image.sprite = Resources.Load("Art/Sprite/Sunnary_Feather") as Sprite;
        //*/
        _canActivate = true;

        return true;
    }


    public override bool StartListen()
    {
        return true;
    }

    public override bool Activate(string activeCase)
    {
        if (canActivate)
            canActivate = false;
        else
            return false;

        Debug.Log("Activate Sunnary Wings.");

        TileChooserManager tileChooser = TileChooserManager.GetInstance();

        IAction actionEndPhase = new LambdaAction(
            () => {
                Owner.AddItem(this);
                canActivate = true;
            });

        // Not Building tiles
        List<PLOT> banned = Plot.BuildingPlot.Union(Plot.TemplePlot).ToList();
        banned.Add(PLOT.TRAVEL);

        tileChooser.Listen(this,
            actionEndPhase,
            banned,
            10f);

        Owner.RemoveItem(this);
        // player active animation


        // DO NOT add item back to Item pool
        // ItemManager.Ins.AddItemToPool(this);

        return true;
    }
    #endregion
    #region fields
    [SerializeField] private bool activated = false;
    #endregion

    private void Start()
    {
        LoadData();
    }

    PLOT? targetPlot;
    PLOT currentPlot { get => Owner.Location_PlotID; }

    ICompletableAction moveAction;

    public PLOT? plot
    {
        get => targetPlot;
        set => targetPlot = value;
    }
    private IAction ActionComplete = null;
    public IAction OnActionComplete { get => ActionComplete; set => ActionComplete = value; }

    public void PerformAction()
    {
        base.Activate("");

        if (plot == null)
            throw new NullReferenceException();

        if (plot == currentPlot)
        {
            PerformOnComplete();
            return;
        }

        System.Action onComplete = () =>
        {
            Plot.plotDictionary[plot.Value].ActiveOnEnter(Owner);
            // Delay the phase a bit.
            PerformOnComplete();
        };

        moveAction = Owner.ActionMoveTo(plot.Value);

        moveAction.OnActionComplete = new LambdaAction(onComplete, moveAction.OnActionComplete);

        moveAction.PerformAction();
        //AnimationEffect(moveAction);
    }

    public void PerformOnComplete()
    {
        OnActionComplete?.PerformAction();
    }
}