using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarketUpgradeUI : MonoBehaviour, UIScreen
{
    PlotConstructionMarket _plot;

    public void Activate()
    {
        gameObject.SetActive(true);
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
    }

    public void SetPlot(Plot plot)
    {
        if(plot is PlotConstructionMarket)
        {
            _plot = plot as PlotConstructionMarket;
        }
        else
        {
            Debug.LogError("Someone set plot that not Construction Temple into Market Upgrade UI");
        }
    }

    public void Upgrade(int level)
    {

    }

    public void Skip()
    {
        TurnDirector.Ins.EndOfPhase();
        StopPhaseUI.Ins.Deactive(PhaseScreens.TempleBuyUI);
    }

    PhaseScreens UIScreen.GetType()
    {
        return PhaseScreens.MarketUpgradeUI;
    }
}
