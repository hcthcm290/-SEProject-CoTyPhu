using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class PlotBuyUI : MonoBehaviour, UIScreen
{
    #region UI Properties

    [SerializeField] protected Text _txtName;
    [SerializeField] protected Text _txtDescription;
    [SerializeField] protected Button _btnBuy;
    [SerializeField] protected Button _btnSkip;
    [SerializeField] protected Text _txtPrice;
    [SerializeField] protected Text _txtPlayerMoney;

    #endregion

    #region Properties
    PlotConstruction _plot;
    public PlotConstruction Plot
    {
        get
        {
            return _plot;
        }

        set
        {
            _plot = value;
        }
    }
    [SerializeField] Player _player;
    [SerializeField] PlotManager _plotManager;
    [SerializeField] bool _canClick = true;
    #endregion

    #region Unity Method
    public void Start()
    {
        _plotManager.OnBuyCallback += BuySuccessCallback;
        _plotManager.OnBuyFailCallback += BuyFailCallback;
    }

    public void Update()
    {   
        _txtName.text = Plot?.Name;
        _txtDescription.text = Plot?.Description;
        _txtPrice.text = "$" + Plot?.PurchasePrice.ToString();
        _txtPlayerMoney.text = "$" + Bank.Ins.MoneyPlayer(_player);

        _btnBuy.enabled = _canClick;
    }
    #endregion

    #region Methods
    public void Buy()
    {
        if (_canClick)
        {
            _canClick = false;
            _plotManager.RequestBuy(_player, Plot);
        }
    }

    public void Skip()
    {
        TurnDirector.Ins.EndOfPhase();
        StopPhaseUI.Ins.Deactive(GetScreenType());
    }

    private void BuySuccessCallback(Player player, PlotConstruction plot)
    {
        if (gameObject.activeSelf == false) return;

        if (player.MinePlayer && plot == Plot)
        {
            Debug.Log("Buy success");
            StopPhaseUI.Ins.Deactive(GetScreenType());
            StopPhaseUI.Ins.Activate(PhaseScreens.MarketUpgradeUI, this._plot);
            _canClick = true;
        }
    }

    private void BuyFailCallback(string reason)
    {
        if (gameObject.activeSelf == false) return;

        // Show on UI the reason they cannot buy
        Debug.Log(reason);
        _canClick = true;

    }

    public virtual PhaseScreens GetScreenType()
    {
        return PhaseScreens.PlotBuyUI;
    }

    public void SetPlot(Plot plot)
    {
        if(plot is PlotConstructionMarket)
        {
            Plot = plot as PlotConstructionMarket;
        }
        else
        {
            Debug.LogError("Someone set plot that not Construction Temple into Temple Buy UI");
        }
    }

    public void Activate()
    {
        gameObject.SetActive(true);
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
    }
    #endregion
}
