using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlotBuyUI : MonoBehaviour
{
    #region UI Properties

    [SerializeField] Text _txtName;
    [SerializeField] Text _txtDescription;
    [SerializeField] Button _btnBuy;
    [SerializeField] Button _btnSkip;
    [SerializeField] Text _txtPrice;
    [SerializeField] Text _txtPlayerMoney;

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
    #endregion

    #region Unity Method
    private void Start()
    {
        _plotManager.OnBuyCallback += BuySuccessCallback;
        _plotManager.OnBuyFailCallback += BuyFailCallback;
    }

    private void Update()
    {   
        _txtName.text = Plot?.Name;
        _txtDescription.text = Plot?.Description;
        _txtPrice.text = "$" + Plot?.PurchasePrice.ToString();
        _txtPlayerMoney.text = "$" + Bank.Ins.MoneyPlayer(_player);
    }
    #endregion

    #region Methods
    public void Buy()
    {
        _plotManager.RequestBuy(_player, Plot);
        _btnBuy.enabled = false;
    }

    public void Skip()
    {
        TurnDirector.Ins.EndOfPhase();
        StopPhaseUI.Ins.Deactive(StopPhaseScreens.PlotBuyUI);
    }

    private void BuySuccessCallback(Player playerId, PlotConstruction plotId)
    {
        Debug.Log("Buy success");
        _btnBuy.enabled = true;
        TurnDirector.Ins.EndOfPhase();
        StopPhaseUI.Ins.Deactive(StopPhaseScreens.PlotBuyUI);
    }

    private void BuyFailCallback(string reason)
    {
        // Show on UI the reason they cannot buy
        Debug.Log(reason);
        _btnBuy.enabled = true;
    }
    #endregion
}
