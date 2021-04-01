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
    List<PlayerElement> listPlayerElements;

    private void Start()
    {
        listPlayerElements = new List<PlayerElement>();
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
}
