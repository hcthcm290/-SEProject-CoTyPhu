using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class TempleBuyUI : MonoBehaviour, UIScreen
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
    PlotConstructionTemple _plot;
    public PlotConstructionTemple Plot
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
        StopPhaseUI.Ins.Deactive(PhaseScreens.TempleBuyUI);
    }

    private void BuySuccessCallback(Player player, PlotConstruction plot)
    {
        if (gameObject.activeSelf == false) return;
        if (player.MinePlayer && plot == Plot)
        {
            Debug.Log("Buy success");
            TurnDirector.Ins.EndOfPhase();
            StopPhaseUI.Ins.Deactive(PhaseScreens.TempleBuyUI);
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

    PhaseScreens UIScreen.GetType()
    {
        return PhaseScreens.TempleBuyUI;
    }

    public void SetPlot(Plot plot)
    {
        if(plot is PlotConstructionTemple)
        {
            Plot = plot as PlotConstructionTemple;
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
