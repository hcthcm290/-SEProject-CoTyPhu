using Photon.Realtime;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerListing : MonoBehaviourPunCallbacks
{   
    [SerializeField]
    Transform ListingTransform;

    [SerializeField]
    PlayerElement elementPrefab;

    [SerializeField]
    List<PlayerElement> listPlayerElements = new List<PlayerElement>();

    [SerializeField]
    Text startGameText;

    bool readyStatus;

    ExitGames.Client.Photon.Hashtable customProp = new ExitGames.Client.Photon.Hashtable();

    private void Start()
    {
    }

    public override void OnEnable()
    {
        base.OnEnable();
        readyStatus = false;
        if(PhotonNetwork.IsMasterClient)
        {
            startGameText.text = "Start Game";
        }
        else
        {
            startGameText.text = "Ready";
        }
    }

    public void RefreshList()
    {
        ClearList();
        foreach (var item in PhotonNetwork.CurrentRoom.Players)
        {
            OnPlayerEnteredRoom(item.Value);
        }
    }

    private void ClearList()
    {
        foreach(var element in listPlayerElements)
        {
            Destroy(element.gameObject);
        }

        //PhotonNetwork.LocalPlayer.CustomProperties;

        listPlayerElements.Clear();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        if(listPlayerElements.Find(x => x.GetPlayer().UserId == newPlayer.UserId) != null)
        {
            Debug.Log("Cannot add duplicate player to list");
            return;
        }

        var player = Instantiate(elementPrefab, ListingTransform);
        player.SetPlayer(newPlayer);

        listPlayerElements.Add(player);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        var player = listPlayerElements.Find(x => x.GetPlayer().UserId == otherPlayer.UserId);
        if(player != null)
        {
            listPlayerElements.Remove(player);
            Destroy(player.gameObject);
        }
    }

    
    public void StartGame()
    {
        if(PhotonNetwork.IsMasterClient)
        {
            if(CheckAllPlayerReady() == false)
            {
                Debug.Log("Cannot start the game. All player must be ready");
                return;
            }
            PhotonNetwork.CurrentRoom.IsOpen = false;
            PhotonNetwork.CurrentRoom.IsVisible = false;
            PhotonNetwork.LoadLevel(1);
        }
        else
        {
            readyStatus = !readyStatus;
            photonView.RPC("RpcPlayerReady", RpcTarget.AllViaServer, PhotonNetwork.LocalPlayer, readyStatus);
        }
    }

    [PunRPC]
    void RpcPlayerReady(Player player, bool value)
    {
        var playerElement = listPlayerElements.Find(x => x.GetPlayer().UserId == player.UserId);

        if(playerElement != null)
        {
            playerElement.SetReadyStatus(value);
        }
    }

<<<<<<< HEAD
    public bool CheckAllPlayerReady()
    {
        foreach(var player in listPlayerElements)
        {
            if(player.GetReadyStatus() == false && !player.GetPlayer().IsMasterClient)
            {
                return false;
            }
        }
        return true;
=======
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        base.OnPlayerPropertiesUpdate(targetPlayer, changedProps);
>>>>>>> 253648b354cffc89c746ed1ebcfc9057d3b4b045
    }
}
