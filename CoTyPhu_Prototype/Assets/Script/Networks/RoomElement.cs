using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class RoomElement : MonoBehaviourPunCallbacks
{
    [SerializeField]
    Text roomName;

    RoomInfo roomInfo;

    private void Update()
    {
        
    }

    private void SetRoomName(string value)
    {
        roomName.text = value;
    }

    public void SetRoomInfo(RoomInfo value)
    {
        this.roomInfo = value;
        SetRoomName(roomInfo.Name);
    }

    public void JoinRoom()
    {
        if (roomInfo != null)
        {
            Debug.Log("Joining room: " + roomInfo.Name);
            PhotonNetwork.JoinRoom(roomInfo.Name);
        }
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined Room");
        SceneManager._ins.MoveToWaitingRoom();
    }
}
