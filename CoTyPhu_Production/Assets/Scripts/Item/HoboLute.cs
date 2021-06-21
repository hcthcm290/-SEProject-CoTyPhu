using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoboLute : BaseItem, IPlotEnterListener, ITurnListener
{
    #region properties
    [SerializeField] StatusHirePriceChange _statusHirePrice;
    [SerializeField] StatusBuybackPriceChange _statusBbPrice;

    [SerializeField] bool isBeginRound = false;
    #endregion

    #region Base class override
    public override bool LoadData()
    {
        return true;
    }

    public override bool StartListen()
    {
        foreach(var plotPair in Plot.plotDictionary)
        {
            var plot = plotPair.Value;

            if(plot is PlotConstruction)
            {
                plot.SubcribePlotEnter(this);
            }
        }

        return true;
    }

    public void StopListen()
    {
        foreach (var plotPair in Plot.plotDictionary)
        {
            var plot = plotPair.Value;

            if (plot is PlotConstruction)
            {
                plot.UnsubcribePlotEnter(this);
            }
        }
    }

    public override bool Remove(bool triggerEvent)
    {
        TurnDirector.Ins.UnsubscribeTurnListener(this);

        Activate("");
        return base.Remove(triggerEvent);
    }

    public override Player Owner 
    { 
        get => base.Owner; 
        set 
        {
            StartListen();
            base.Owner = value;
        }
    }

    public override List<Vector3> TargetLocations()
    {
        return new List<Vector3>();
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPlotEnter(Player player, Plot plot)
    {
        if(player == Owner)
        {
            if(!(plot is PlotConstruction))
            {
                Debug.LogError("Subcribe to wrong plot");
            }

            PlotConstruction plotConstruction = plot as PlotConstruction;

            if(plotConstruction.Owner != null && plotConstruction.Owner != this.Owner)
            {
                Owner.RemoveItem(this);

                _statusHirePrice.targetPlot = plotConstruction;
                _statusBbPrice.targetPlot = plotConstruction;

                Debug.Log("Hobo's Lute activated");

                _statusBbPrice.StartListen();
                _statusHirePrice.StartListen();

                StopListen();

                TurnDirector.Ins.SubscribeTurnListener(this);
            }
        }
    }

    public void OnBeginTurn(int idPlayer)
    {
        if(idPlayer != Owner.Id)
        {
            isBeginRound = true;
        }
    }

    public void OnEndTurn(int idPlayer)
    {
        // when it's end a round, remove item's status
        if (idPlayer == Owner.Id && isBeginRound)
        {
            _statusHirePrice.Remove(true);
            _statusBbPrice.Remove(true);
            this.Remove(true);
        }
    }
}
