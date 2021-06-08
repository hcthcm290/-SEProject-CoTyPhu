using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomInfoCard : MonoBehaviour
{
    [SerializeField] Text ownerNameText;
    [SerializeField] Text playerCountText;
    public string roomName;

    public delegate void OnClickFunction(string roomName);
    public event OnClickFunction onClicked;

    public void SetInfo(string roomName, string ownerName, int playerCount)
    {
        ownerNameText.text = ownerName;
        playerCountText.text = playerCount + "/4";
        this.roomName = roomName;
    }

    public void SetInfo(RoomInfo roomInfo)
    {
        string ownerName = roomInfo.CustomProperties["ownerName"] as string;
        int playerCount = roomInfo.PlayerCount;
        string roomName = roomInfo.Name;

        SetInfo(roomName, ownerName, playerCount);
    }

    public void Click()
    {
        onClicked?.Invoke(roomName);
    }
}
