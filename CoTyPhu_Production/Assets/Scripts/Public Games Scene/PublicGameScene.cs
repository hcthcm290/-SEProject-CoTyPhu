using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class PublicGameScene : MonoBehaviourPunCallbacks
{
    #region UI properties
    [SerializeField] GameObject ListRoomContent;
    [SerializeField] RoomInfoCard RoomInfoCardPrefab;
    [SerializeField] Button refreshButton;
    #endregion

    static List<RoomInfoCard> listRoomInfoCards;
    static List<RoomInfo> listRoomInfos;

    [SerializeField]
    List<RoomInfoCard> listRoomInfoCardsDebug;

    private void Start()
    {
        if(listRoomInfoCards == null)
        {
            listRoomInfoCards = new List<RoomInfoCard>();
        }

        if(listRoomInfos == null)
        {
            listRoomInfos = new List<RoomInfo>();
        }

        listRoomInfoCardsDebug = listRoomInfoCards;

        Refresh();
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        listRoomInfos = roomList;
        refreshButton.gameObject.SetActive(true);
    }

    public static void UpdateRoomList(List<RoomInfo> roomList)
    {
        listRoomInfos = roomList;
    }

    public void JoinRoom(string roomName)
    {
        Debug.Log("prepare to join room: " + roomName);
        PhotonNetwork.JoinRoom(roomName);
    }

    public void Refresh()
    {
        foreach (var item in listRoomInfoCards)
        {
            if(item != null)
            {
                Destroy(item.gameObject);
            }
        }
        listRoomInfoCards.Clear();

        foreach (var item in listRoomInfos)
        {
            if (item.PlayerCount == 0) continue;

            var newRoomInfoCard = Instantiate(RoomInfoCardPrefab, ListRoomContent.transform);
            newRoomInfoCard.SetInfo(item);
            newRoomInfoCard.onClicked += JoinRoom;
            newRoomInfoCard.onClicked += (_) => SoundManager.Ins.Play(AudioClipEnum.Select);
            listRoomInfoCards.Add(newRoomInfoCard);
        }

        refreshButton.gameObject.SetActive(false);
        listRoomInfoCardsDebug = listRoomInfoCards;
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        Debug.Log("joined room: " + PhotonNetwork.CurrentRoom.Name);
        Debug.Log("prepare loading waiting room scene");
        SceneManager.LoadScene("WaitingRoomScene");
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        base.OnJoinRoomFailed(returnCode, message);
        Debug.LogError(message);
    }

    public void goMainMenuScene()
    {
        Debug.Log("prepare loading main menu scene");
        SceneManager.LoadScene("MainMenuScene");
    }
}
