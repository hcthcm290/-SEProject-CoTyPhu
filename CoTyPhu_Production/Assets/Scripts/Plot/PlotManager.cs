using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

/// <summary>
/// This object handle all the buy, rebuy, sell, ... request, callback related to plot
/// </summary>
public class PlotManager : MonoBehaviourPun
{
    #region Event
    public delegate void Buy(Player playerId, PlotConstruction plotId);
    public event Buy OnBuyCallback;

    public delegate void BuyFail(string msg);
    public event BuyFail OnBuyFailCallback;
    #endregion

    #region Properties

    #endregion

    #region Unity methods

    private void Update()
    {
        
    }

    #endregion

    #region Method
    [PunRPC]
    private void RequestBuyServer(int playerId, int plotId, string clientID)
    {
        Player player = TurnDirector.Ins.GetPlayer(playerId);
        PlotConstruction plot = Plot.plotDictionary[(PLOT)plotId] as PlotConstruction;
        var cs = PhotonNetwork.PlayerList;
        Photon.Realtime.Player client = PhotonNetwork.PlayerList.Single(x => x.UserId == clientID);

        if(player == null | plot == null)
        {
            photonView.RPC("BuyFailCallback", client, "playerID " + playerId + " or plotID " + plotId + " not found");
            return;
        }
        else if(Bank.Ins.MoneyPlayer(player) < plot.PurchasePrice)
        {
            photonView.RPC("BuyFailCallback", client, "Not enough money");
            return;
        }
        else
        {
            photonView.RPC("BuySuccessCallback", RpcTarget.All, playerId, plotId);
            return;
        }
    }

    [PunRPC]
    public void BuySuccessCallback(int playerId, int plotId)
    {
        Player player = TurnDirector.Ins.GetPlayer(playerId);
        PlotConstruction plot = Plot.plotDictionary[(PLOT)plotId] as PlotConstruction;

        plot.Owner = player;
        Bank.Ins.TakeMoney(player, plot.PurchasePrice);

        OnBuyCallback?.Invoke(player, plot);
    }

    [PunRPC]
    public void BuyFailCallback(string msg)
    {
        OnBuyFailCallback?.Invoke(msg);
    }

    public void RequestBuy(Player player, PlotConstruction plot)
    {
        if(Bank.Ins.MoneyPlayer(player) >= plot.PurchasePrice)
        {
            photonView.RPC("RequestBuyServer", RpcTarget.MasterClient, player.Id, (int)plot.Id, PhotonNetwork.LocalPlayer.UserId);
        }
        else
        {
            OnBuyFailCallback?.Invoke("Not enough money");
        }
    }

    #endregion
}
