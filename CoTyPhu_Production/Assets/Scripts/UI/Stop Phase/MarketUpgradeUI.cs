using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MarketUpgradeUI : MonoBehaviour, UIScreen
{
    #region UI Properties
    [SerializeField] Text[] _txtPriceLvls;
    [SerializeField] Button[] _buttonUpgrade;
    #endregion

    #region Properties
    PlotConstructionMarket _plot;
    [SerializeField] PlotManager _plotManager;
    [SerializeField] Player player;
    #endregion

    #region Methods UIScreen
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

            if(_txtPriceLvls != null)
            {
                for(int i = 0; i< _txtPriceLvls.Length; i++)
                {
                    var txtPriceLvl = _txtPriceLvls[i];

                    if(_plot.Level <= i)
                    {
                        if (txtPriceLvl != null)
                        {
                            txtPriceLvl.text = _plot.UpgradeFee(_plot.Level, i + 1).ToString();
                        }
                        if(_buttonUpgrade[i] != null)
                        {
                            _buttonUpgrade[i].gameObject.SetActive(true);
                        }
                    }
                    else
                    {
                        if (txtPriceLvl != null)
                        {
                            txtPriceLvl.text = "";
                        }
                        if (_buttonUpgrade[i] != null)
                        {
                            _buttonUpgrade[i].gameObject.SetActive(false);
                        }
                    }
                }
            }

            // Only show upgrade button level 4 when plot has been upgrade to level 3
            if(_plot.Level < 3)
            {
                _buttonUpgrade[3].gameObject.SetActive(false);
            }
            else
            {
                _buttonUpgrade[3].gameObject.SetActive(true);
            }
        }
        else
        {
            Debug.LogError("Someone set plot that not Construction Temple into Market Upgrade UI");
        }
    }

    PhaseScreens UIScreen.GetScreenType()
    {
        return PhaseScreens.MarketUpgradeUI;
    }
    #endregion

    #region Methods
    public void Upgrade(int level)
    {
        _plotManager.RequestUpgrade(player.Id, _plot.Id, level);
    }

    private void OnUpgradeSuccess(Player player, PlotConstructionMarket plot, int level)
    {
        if (gameObject.activeSelf == false) return;

        Skip();
    }

    private void OnUpgradeFail(string msg)
    {
        if (gameObject.activeSelf == false) return;
        Debug.Log("msg");
    }

    public void Skip()
    {
        TurnDirector.Ins.EndOfPhase();
        StopPhaseUI.Ins.Deactive(PhaseScreens.MarketUpgradeUI);
    }
    #endregion

    #region Unity Methods
    private void Start()
    {
        _plotManager.OnUpgradeSuccess += OnUpgradeSuccess;
        _plotManager.OnUpgradeFail += OnUpgradeFail;
    }
    #endregion
}
