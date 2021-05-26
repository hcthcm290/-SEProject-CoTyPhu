using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurningDice : BaseItem, IPlotEnterListener, IPayPlotFeeListener
{
    #region Base class override
    public override bool LoadData()
    {
        return true;
    }

    public override bool StartListen()
    {

        foreach (var plotPair in Plot.plotDictionary)
        {
            var plot = plotPair.Value;

            if(plot is PlotConstruction)
            {
                plot.SubcribePlotEnter(this);
            }
        }
        return true;
    }

    public override bool Activate(string activeCase)
    {
        StartListen();
        Owner.RemoveItem(this);
        // player active animation


        // Add item back to Item pool
        ItemManager.Ins.AddItemToPool(this);

        activated = true;
        return base.Activate(activeCase);
    }
    #endregion
    #region fields
    [SerializeField] StatusHirePriceChange _statusPrefab;
    [SerializeField] private bool activated = false;
    [SerializeField] private StatusHirePriceChange activeStatus;
    #endregion

    void StopListen()
    {
        foreach (var plotPair in Plot.plotDictionary)
        {
            var plot = plotPair.Value;

            if (plot is PlotConstruction)
            {
                plot.UnsubcribePlotEnter(this);
            }
        }
        return;
    }

    #region Handle event
    public void OnPlotEnter(Player player, Plot plot)
    {
        Debug.Log("Burning dice: On plot enter");
        if(player == Owner)
        {
            if(plot is PlotConstruction)
            {
                PlotConstruction plotConstruction = plot as PlotConstruction;
                if(plotConstruction.Owner == this.Owner)
                {
                    Debug.Log("Burning Dice: on my plot enter");
                    // Add 1 mana
                    Owner.ChangeMana(1);

                    // Player fancy animation

                    // Create status
                    var newStatus = Instantiate(_statusPrefab);
                    newStatus.hirePriceChange = 0.5f;
                    newStatus.targetPlot = plotConstruction;
                    newStatus.StartListen();

                    activeStatus = newStatus;

                    StopListen();

                    // subcribe event for handle remove status
                    plotConstruction.SubcribePayPlotFee(this);
                }
            }
        }
    }

    public void OnPayPlotFee(Player player, PlotConstruction plot)
    {
        if(plot != activeStatus.targetPlot)
        {
            Debug.Log("Subcribe to wrong plot");
            return;
        }
        if(player != Owner)
        {
            activeStatus.targetPlot.UnsubcribePayPlotFee(this);
            activeStatus.Remove(true);
            Destroy(this.gameObject);
        }
    }

    #endregion
}
