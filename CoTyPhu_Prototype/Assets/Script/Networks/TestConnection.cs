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
        var gameVersion = MasterManager.GameManager.gameVersion;
        Debug.Log("Connecting to using setting status: " + PhotonNetwork.ConnectUsingSettings().ToString());
        connected = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(connected == true)
        {
            Debug.Log("Disconnecting from server");
            PhotonNetwork.Disconnect();
        }
    }

    public override void OnConnected()
    {
        Debug.Log("Connected");
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Master");
        connected = true;
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("Disconnected from server: " + cause.ToString());
        connected = false;
    }
}
