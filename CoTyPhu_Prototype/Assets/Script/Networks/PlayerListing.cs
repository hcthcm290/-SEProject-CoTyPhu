using Photon.Realtime;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerListing : MonoBehaviourPunCallbacks
{   
    [SerializeField]
    Transform ListingTransform;

    [SerializeField]
    PlayerElement elementPrefab;

    [SerializeField]
    List<PlayerElement> listPlayerElements = new List<PlayerElement>();

    bool readyStatus;

    private void Start()
    {
    }

    public override void OnEnable()
    {
        base.OnEnable();
        readyStatus = false;
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
}
