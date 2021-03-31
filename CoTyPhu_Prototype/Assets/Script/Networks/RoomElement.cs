using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomElement : MonoBehaviour
{
    [SerializeField]
    Text roomName; 

    public void SetRoomName(string value)
    {
        roomName.text = value;
    }
}
