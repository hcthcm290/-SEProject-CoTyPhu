using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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

    public void Refresh()
    {
        foreach(var item in listRoomInfoCards)
        {
            Destroy(item.gameObject);
        }
        listRoomInfoCards.Clear();

        foreach (var item in listRoomInfos)
        {
            var newRoomInfoCard = Instantiate(RoomInfoCardPrefab, ListRoomContent.transform);
            newRoomInfoCard.SetInfo(item);
            listRoomInfoCards.Add(newRoomInfoCard);
        }

        refreshButton.gameObject.SetActive(false);
    }
}
