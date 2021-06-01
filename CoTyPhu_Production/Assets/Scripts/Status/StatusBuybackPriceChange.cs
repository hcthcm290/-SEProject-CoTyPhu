using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusBuybackPriceChange : BaseStatus, IBuyBackPriceChange
{
    #region Base class override
    public override bool ExcuteAction()
    {
        return true;
    }

    public override bool LoadData()
    {
        return true;
    }

    public override bool StartListen()
    {
        if(targetPlot == null)
        {
            Debug.LogError("Cannot listen on null target plot");
            return false;
        }
        else
        {
            targetPlot.AddStatus(this);
            return true;
        }
    }

    public override bool Remove(bool triggerEvent)
    {
        targetPlot.RemoveStatus(this);
        base.Remove(triggerEvent);
        Destroy(this.gameObject);

        return true;
    }
    #endregion

    #region properties
    public PlotConstruction targetPlot;

    [SerializeField] float _buybackPriceChange;
    public float buyBackPriceChange 
    { 
        get 
        {
            return _buybackPriceChange;
        }
        set
        {
            _buybackPriceChange = value;
        }
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

    public float GetBuyBackPriceChange(float basePrice, float delta)
    {
        delta += basePrice * buyBackPriceChange;
        return delta;
    }
}
