using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IcingDice : BaseItem, ITurnListener, IPlotEnterListener
{
    [SerializeField] StatusHirePriceChange status;

    private bool activated;
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

            if (plot is PlotConstruction)
            {
                plot.SubcribePlotEnter(this);
            }
        }
        return true;
    }

    private void DecreasePrice()
    {  
        foreach (var plotPair in Plot.plotDictionary)
        {
            var plot = plotPair.Value;
            if (plot is PlotConstruction)
            {
                (plot as PlotConstruction).AddStatus(status);
            }                
        }
    }
    
    public override bool Remove(bool triggerEvent)
    {
        foreach (var plotPair in Plot.plotDictionary)
        {
            var plot = plotPair.Value;
            if (plot is PlotConstruction)
            {
                (plot as PlotConstruction).RemoveStatus(status);
            }
        }
        base.Remove(triggerEvent);
        Destroy(this.gameObject);
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
        DecreasePrice();
        TurnDirector.Ins.SubscribeTurnListener(this);

        return base.Activate(activeCase);
    }
    #endregion
    #region Unity Method
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnBeginTurn(int idPlayer)
    {
        if (idPlayer==this.Owner.Id)
        {
            Remove(true);
        }    
    }

    public void OnEndTurn(int idPlayer)
    {
        //throw new System.NotImplementedException();
    }

    public void OnPlotEnter(Player player, Plot plot)
    {
       // throw new System.NotImplementedException();
    }
    #endregion
}
