using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UseFreeCardUI : MonoBehaviour, UIScreen
{
    [SerializeField] Button _useCardButton;
    [SerializeField] Button _skipButton;
    [SerializeField] Player _player;
    [SerializeField] PlotManager _plotManager;
    [SerializeField] Text _txtReleaseFee;
    [SerializeField] Text _txtPlayerMoney;
    [SerializeField] Button _useMoney;

    int _playerMoney;
    int _releaseFee;   

    PlotPrison _plot;
    public void Activate()
    {
        this.gameObject.SetActive(true);
    }

    public void Deactivate()
    {
        this.gameObject.SetActive(false);
    }

    public void SetPlot(Plot plot)
    {
        if(!(plot is PlotPrison))
        {
            Debug.LogError("Cannot set plot that not prison to UseFreeCard UI");
            return;
        }
        else
        {
            _plot = plot as PlotPrison;
        }

    }

    PhaseScreens UIScreen.GetScreenType()
    {
        return PhaseScreens.FreeCardUI;
    }

    private void Start()
    {
        _useCardButton.onClick.AddListener(UseCard);
        _skipButton.onClick.AddListener(Skip);
        _useMoney.onClick.AddListener(UseMoney);
    }

    public void UseCard()
    {
        // call
        _plotManager.RequestRelease(_player);

        // Play fancy animation


        StopPhaseUI.Ins.Deactive(PhaseScreens.FreeCardUI);
    }

    public void Update()
    {
        if(_player != null)
        {
            _playerMoney = Bank.Ins.MoneyPlayer(_player);

            if (_plot != null)
            {
                _releaseFee = _plot.GetReleaseFee(_player);
            }
        }

        _txtPlayerMoney.text = _playerMoney.ToString();
        _txtReleaseFee.text = _releaseFee.ToString();
    }

    public void UseMoney()
    {
        if(_playerMoney > _releaseFee)
        {
            _plotManager.RequestReleaseWithMoney(_player, UseMoneySuccess, UseMoneyFail);
        }
        else
        {
            // TODO
            // Show the msg for player

            //
            Debug.Log("Don't have enough money");
        }
    }

    void UseMoneySuccess()
    {
        if (gameObject.activeSelf == false) return;
        StopPhaseUI.Ins.Deactive(PhaseScreens.FreeCardUI);
    }

    void UseMoneyFail(string reason)
    {
        if (gameObject.activeSelf == false) return;
        // TODO
        // Show the msg for player

        //
        Debug.Log("Don't have enough money");
    }

    public void Skip()
    {
        StopPhaseUI.Ins.Deactive(PhaseScreens.FreeCardUI);
    }
}
