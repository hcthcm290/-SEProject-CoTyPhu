using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class WaitingRoom : MonoBehaviourPunCallbacks
{
    #region UI properties
    [SerializeField] GameObject listPlayerInfoContent;
    [SerializeField] PlayerInfoCard playerInfoCardPrefab;
    [SerializeField] Text roomName;
    [SerializeField] Button startButton;
    #endregion

    public List<Photon.Realtime.Player> playersInRoom;
    Dictionary<Photon.Realtime.Player, PlayerInfoCard> listPlayerInfoCard;

    private void Start()
    {
        playersInRoom = new List<Photon.Realtime.Player>();
        listPlayerInfoCard = new Dictionary<Photon.Realtime.Player, PlayerInfoCard>();
        roomName.text = PhotonNetwork.CurrentRoom.Name;
        foreach (var player in PhotonNetwork.PlayerList)
        {
            OnPlayerEnteredRoom(player);
        }
        if(!PhotonNetwork.IsMasterClient)
        {
            startButton.gameObject.SetActive(false);
        }
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        playersInRoom.Add(newPlayer);
        var newPlayerInfoCard = Instantiate(playerInfoCardPrefab, listPlayerInfoContent.transform);
        newPlayerInfoCard.SetInfo(newPlayer);
        listPlayerInfoCard.Add(newPlayer, newPlayerInfoCard);
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);
        playersInRoom.Remove(otherPlayer);
        PlayerInfoCard otherCard = listPlayerInfoCard[otherPlayer];
        Destroy(otherCard.gameObject);
    }

    public void LeaveRoom()
    {
        Debug.Log("prepare to leave room");
        PhotonNetwork.LeaveRoom(false);
    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        Debug.Log("left room");
        Debug.Log("prepare loading main menu scene");
        SceneManager.LoadScene("MainMenuScene");
    }

    public void StartGame()
    {
        SoundManager.Ins.Play(AudioClipEnum.Select);
        Debug.Log("prepare loading merchant scene picking");
        PhotonNetwork.LoadLevel("MerchantPickingScene");
    }
}
