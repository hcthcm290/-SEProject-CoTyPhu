using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public interface IPlotChooserAction : IAction
{
    public PLOT? plot
    {
        get;
        set;
    }
}

/// <summary>
/// Call Listen with the action that needs a chosen plot.
/// After Chosen (which will result in calling ListeningAction),
/// or Timeout (which will result in calling defaultAction),
/// The action will be executed.
/// </summary>
public class TileChooserManager : MonoBehaviourPunCallbacks
{
    private const float DEFAULT_TIME_TO_CHOOSE_PLOT = 10f;
    public float timeToChoose;
    public TileChooserManager()
    {
        Locator.MarkInstance(this);
    }

    public void Listen(IPlotChooserAction action, IAction defaultAction,
        List<PLOT> excludedPlots = null,
        float timeToChoose = DEFAULT_TIME_TO_CHOOSE_PLOT)
    {
        ListeningAction = action;
        this.defaultAction = defaultAction;
        this.timeToChoose = timeToChoose;

        // Master initiate the skip count down
        if (PhotonNetwork.IsMasterClient)
        {
            StartTimer();
            SkipButtonUI.GetInstance().ListenClick(new LambdaAction(() =>
            {
                photonView.RPC("OnTimeoutEvent", RpcTarget.All);
            }));
        }
        if (!TurnDirector.Ins.IsMyTurn())
            return;
        // Player choose the plot
        CameraTopDown cameraComponent = CameraTopDown.GetInstance();
        cameraComponent.Active = true;
        cameraComponent.ListenTargetReached(new LambdaAction(() =>
        {
            gameObject.SetActive(true);

            if (excludedPlots != null)
                foreach (PLOT plot in excludedPlots)
                {
                    TileChooserButton.buttonDictionary[plot].button.enabled = false;
                }

            postChosenTile = new LambdaAction(() =>
            {
                cameraComponent.Active = true;
                gameObject.SetActive(false);

                if (excludedPlots != null)
                    foreach (PLOT plot in excludedPlots)
                    {
                        TileChooserButton.buttonDictionary[plot].button.enabled = true;
                    }
            });
        }));
    }

    private void StartTimer()
    {
        SkipButtonUI skip = SkipButtonUI.GetInstance();

        skip.MaxTime = timeToChoose;
        skip.EnableTimerOnly();
    }

    private IAction postChosenTile = null;
    private IAction defaultAction = null;
    private IPlotChooserAction listeningAction = null;
    private int plot = -1; 

    public int Plot
    {
        get => plot;
        set
        {
            plot = value;
            if (listeningAction != null && plot > -1)
                PerformOnSelectEvent();
        }
    }
    private IPlotChooserAction ListeningAction
    {
        get => listeningAction;
        set
        {
            listeningAction = value;
            if (listeningAction != null && plot > -1)
                PerformOnSelectEvent();
        }
    }

    public void ChooseTile(int chosenTile)
    {
        photonView.RPC("PublishChoiceMaster", RpcTarget.MasterClient, chosenTile);
    }

    #region Event
    [PunRPC]
    private void PublishChoice(int chosenPlot)
    {
        Plot = chosenPlot;
    }
    [PunRPC]
    private void PublishChoiceMaster(int chosenPlot)
    {
        var skip = SkipButtonUI.GetInstance();
        // If Master recieve the message too late
        if (!skip.gameObject.activeSelf)
            return;

        skip.Disable();
        photonView.RPC("PublishChoice", RpcTarget.All, chosenPlot);
    }
    [PunRPC]
    private void OnTimeoutEvent()
    {
        defaultAction?.PerformAction();
        ResetSelf();
    }

    private void ResetSelf()
    {
        postChosenTile?.PerformAction();

        postChosenTile = null;
        ListeningAction = null;
        defaultAction = null;
        Plot = -1;
    }
    public void PerformOnSelectEvent()
    {
        ListeningAction.plot = (PLOT)Plot;
        ListeningAction.PerformAction();

        //*
        ResetSelf();
        /*/
        postChosenTile?.PerformAction();

        postChosenTile = null;
        defaultAction = null;
        ListeningAction = null;
        Plot = -1;
        //*/
    }
    #endregion

    public static TileChooserManager GetInstance()
    {
        return Locator.GetInstance<TileChooserManager>();
    }

    private void Start()
    {
        StartCoroutine(WaitDisableFirstFrame());
    }

    private IEnumerator WaitDisableFirstFrame()
    {
        yield return new WaitForEndOfFrame();

        this.gameObject.SetActive(false);
    }
}
