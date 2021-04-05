using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class RoomNameAsync : MonoBehaviour
{
    [SerializeField]
    Text roomName;

    // Update is called once per frame
    void Update()
    {
        var Room = PhotonNetwork.CurrentRoom;

        if(Room == null)
        {
            roomName.text = "RoomName:";
        }
        else
        {
            roomName.text = "RoomName: " + Room.Name;
        }
    }
}
