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

    PhaseScreens UIScreen.GetType()
    {
        return PhaseScreens.FreeCardUI;
    }

    private void Start()
    {
        _useCardButton.onClick.AddListener(UseCard);
        _skipButton.onClick.AddListener(Skip);
    }

    public void UseCard()
    {
        // call
        _plotManager.RequestRelease(_player);

        // Play fancy animation


        // End of phase

        StopPhaseUI.Ins.Deactive(PhaseScreens.FreeCardUI);
    }

    public void Skip()
    {
        StopPhaseUI.Ins.Deactive(PhaseScreens.FreeCardUI);
    }
}
