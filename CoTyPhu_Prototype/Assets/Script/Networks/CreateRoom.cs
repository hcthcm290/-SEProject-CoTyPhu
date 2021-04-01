using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateRoom : MonoBehaviourPunCallbacks
{
    [SerializeField]
    InputField RoomName;

    public void Create()
    {
        if (!PhotonNetwork.IsConnected)
        {
            Debug.Log("Cannot create room because client is not connected to server");
            return;
        }
        Debug.Log("Creating room with name: " + RoomName.text);
        RoomOptions options = new RoomOptions();
        options.MaxPlayers = 4;
        options.IsVisible = true;
        options.EmptyRoomTtl = 0;
        PhotonNetwork.CreateRoom(RoomName.text, options, TypedLobby.Default);
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("Room created");
        SceneManager._ins.MoveToWaitingRoom();
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.LogError("Room created Failed\nReturn Code: " + returnCode.ToString() + "\nMessage: " + message);
    }
}
