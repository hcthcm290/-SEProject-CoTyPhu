using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StopPhaseScreens
{
    PlotBuyUI,
}

public class StopPhaseUI : MonoBehaviour
{
    #region UI Properties
    [SerializeField] PlotBuyUI _plotBuyUI;
    #endregion

    #region Properties
    private static StopPhaseUI _ins;
    public static StopPhaseUI Ins
    {
        get { return _ins; }
    }
    #endregion

    #region Methods
    public void Activate(StopPhaseScreens screen, Plot plot)
    {
        switch (screen)
        {
            case StopPhaseScreens.PlotBuyUI:
                _plotBuyUI.gameObject.SetActive(true);
                _plotBuyUI.Plot = plot as PlotConstruction;
                break;
        }
    }

    public void Deactive(StopPhaseScreens screen)
    {
        switch (screen)
        {
            case StopPhaseScreens.PlotBuyUI:
                _plotBuyUI.gameObject.SetActive(false);
                break;
        }
    }
    #endregion

    #region Unity Methods
    private void Start()
    {
        _ins = this;
    }

    #endregion
}
