using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class TestConnection: MonoBehaviourPunCallbacks
{
    bool connected;

    // Start is called before the first frame update
    void Start()
    {
        var gameVersion = "1.0.0";//MasterManager.GameManager.gameVersion;
        //var nickName = MasterManager.GameManager.NickName;
        string nickName = Random.Range(0, 9999).ToString();
        PhotonNetwork.GameVersion = gameVersion;
        PhotonNetwork.NickName = nickName;
        Debug.Log("Connecting using setting status: " + PhotonNetwork.ConnectUsingSettings().ToString());
        Debug.Log("Connecting to Master with Nickname: " + PhotonNetwork.LocalPlayer.NickName + " \nOn game version: " + PhotonNetwork.GameVersion);

        connected = false;
    }

    // Update is called once per frame
    void Update()
    {
        //if (connected == true)
        //{
        //    Debug.Log("Disconnecting from server");
        //    PhotonNetwork.Disconnect();
        //}

        if(!PhotonNetwork.IsConnectedAndReady)
        {
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public override void OnConnected()
    {
        Debug.Log("Connected");
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Master with Nickname: " + PhotonNetwork.LocalPlayer.NickName + " \nOn game version: " + PhotonNetwork.GameVersion);
        connected = true;

        PhotonNetwork.JoinLobby();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("Disconnected from server: " + cause.ToString());
        connected = false;
    }
}
