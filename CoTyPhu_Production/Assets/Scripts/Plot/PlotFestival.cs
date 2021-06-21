using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// PLOT.FESTIVAL (16) INCREASE THE ENTRY FEE ( BY AN AMOUNT ) OF A CONSTRUCTION PLOT WHEN ENTERED 
/// </summary>
public class PlotFestival : Plot
{
	//  Events ----------------------------------------


	//  Properties ------------------------------------
	public float BonusPercentage { get => _bonusValuePercent * 100; }
    [SerializeField] StatusHirePriceChange _statusPrefab;


    //  Fields ----------------------------------------
    private float _bonusValuePercent;


	//  Initialization --------------------------------
	public PlotFestival(PLOT id, string name, string description, int bonusValue) : base(id, name, description)
	{
		this._bonusValuePercent = bonusValue;
	}


    //  Methods ---------------------------------------
	private void ApplyFestivalEffect(PLOT plot)
	{
		//TODO: Increase EntryFee of a Construction by _bonusValue
	}

	public override IAction ActionOnEnter(Player obj)
    {
		return new LambdaAction(() =>
		{
            // get Action.
            FestivalAction action = new FestivalAction();
            var defaultAction = base.ActionOnEnter(obj);
            action.player = obj;
            action.statusPrefab = this._statusPrefab;

            // get Plots player owns.
            int amountOfValidPlot = 0;
            List<PLOT> banned = Plot.plotDictionary.Keys.ToList();
            var candidates = Plot.BuildingPlot.Union(Plot.TemplePlot);
            foreach (var item in candidates)
            {
                if (Plot.plotDictionary[item] is PlotConstruction)
                {
                    PlotConstruction plot = Plot.plotDictionary[item] as PlotConstruction;
                    if (plot.Owner == obj)
                    {
                        amountOfValidPlot++;
                        banned.Remove(item);
                    }
                }
            }

            if (amountOfValidPlot == 0)
            {
                Debug.Log("Festival Plot: No targets for festival. Exiting.");
                defaultAction.PerformAction();
                return;
            }

            if (action.OnActionComplete == null)
                action.OnActionComplete = defaultAction;
            else
                action.OnActionComplete = action.OnActionComplete.Add(defaultAction);

            TileChooserManager.GetInstance().Listen(action, action, banned, 10f);
        });
	}

    //  Event Handlers --------------------------------
}

public class FestivalAction : IPlotChooserAction, ITurnListener, ICompletableAction
{
    PLOT? _plot = null;
    public PLOT? plot { get => _plot; set => _plot = value; }
    private IAction OnComplete;
    public IAction OnActionComplete { get => OnComplete; set => OnComplete = value; }

    PlotConstruction target;
    public Player player;
    public StatusHirePriceChange statusPrefab;
    StatusHirePriceChange activeStatus;

    public void PerformAction()
    {
        if (plot == null)
        {
            Debug.LogError("Festival: Didn't choose a plot");
            // TODO: continue with the first plot of the player
            var candidates = Plot.BuildingPlot.Union(Plot.TemplePlot);
            foreach (var item in candidates)
            {
                if (Plot.plotDictionary[item] is PlotConstruction)
                {
                    if ((Plot.plotDictionary[item] as PlotConstruction).Owner == player)
                    {
                        plot = item;
                        break;
                    }
                }
                else
                {
                    Debug.LogError("Uh... why did this happen? I don't know.");
                }
            }

            //if(plot == null)
            //{
            //    if(player.MinePlayer)
            //    {
            //        TurnDirector.Ins.EndOfPhase();
            //    }
            //}
        }

        target = Plot.plotDictionary[plot.Value] as PlotConstruction;
        if (target.Owner == player)
        {
            Debug.Log("Festival: OnStop action activate.");

            // Plot target fancy animation

            // Create status
            var newStatus = GameObject.Instantiate(statusPrefab);
            newStatus.hirePriceChange = 1.0f;
            newStatus.targetPlot = target;
            newStatus.StartListen();
            
            activeStatus = newStatus;

            // subcribe event for handle remove status
            TurnDirector.Ins.SubscribeTurnListener(this);
        }
        else
        {
            Debug.Log("Festival: Invalid Plot chosen.");
        }

        PerformOnComplete();
    }

    public void OnBeginTurn(int idPlayer)
    {
        if (player.Id == idPlayer || player.HasLost || target.Owner != player)
        {
            TurnDirector.Ins.UnsubscribeTurnListener(this);
            activeStatus.Remove(true);
            Debug.Log("Festival: player" + idPlayer + "'s festival at " + plot.ToString() + " has ended");
        }
    }

    public void OnEndTurn(int idPlayer)
    {
        return;
    }

    public void PerformOnComplete()
    {
        OnComplete?.PerformAction();
    }
}