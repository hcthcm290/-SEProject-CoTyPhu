using UnityEngine;
using System.Collections.Generic;
using System;

/// <summary>
/// PLOT.TRAVEL (24) MOVE PLAYER IMMEDIATELY TO A SELECTED PLOT 
/// </summary>
public class PlotTravel: Plot
{
	//  Events ----------------------------------------


	//  Properties ------------------------------------


	//  Fields ----------------------------------------


	//  Initialization --------------------------------
	public PlotTravel(PLOT id, string name, string description) : base(id, name, description) { }


    //  Methods ---------------------------------------
    private void PerformTravel(Plot destination)
    {
        //TODO: Move immediately to destination
    }

    public override IAction ActionOnEnter(Player obj)
    {
        IAction result = new LambdaAction(() => 
        {
            NotifyPlotEnter(obj);

            var tileChooser = TileChooserManager.GetInstance();

            ActionTravel travel = new ActionTravel(obj);

            IAction actionEndPhase = new LambdaAction(() => TurnDirector.Ins.EndOfPhase());

            tileChooser.Listen(travel, 
                actionEndPhase,
                null, 
                10f);
        });

        NotifyPlotEnter(obj);

        return result;
    }


    //  Event Handlers --------------------------------
}

class ActionTravel : IPlotChooserAction, ICompletableAction
{
    Player targetPlayer;
    PLOT? targetPlot;
    PLOT currentPlot { get => targetPlayer.Location_PlotID; }

    public Player.ActionPlayerMove action;

    public ActionTravel(Player target)
    {
        targetPlayer = target;
        targetPlot = null;
    }

    public PLOT? plot 
    { 
        get => targetPlot; 
        set => targetPlot = value;
    }
    private IAction ActionComplete = null;
    public IAction OnActionComplete { get => ActionComplete; set => ActionComplete = value; }

    public void PerformAction()
    {
        if (plot == null)
            throw new NullReferenceException();

        if (plot == currentPlot)
        {
            TurnDirector.Ins.EndOfPhase();
            PerformOnComplete();
            return;
        }

        int dif = ((int)plot - (int)currentPlot + Plot.PLOT_AMOUNT) % Plot.PLOT_AMOUNT;

        System.Action onComplete = () =>
        {
            Plot.plotDictionary[plot.Value].ActiveOnEnter(targetPlayer);
            PerformOnComplete();
        };

        action = new Player.ActionPlayerMove(targetPlayer, new List<int> { dif }, onComplete);

        action.PerformAction();
    }

    public void PerformOnComplete()
    {
        OnActionComplete?.PerformAction();
    }
}