using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class RoomListing : MonoBehaviourPunCallbacks
{
    [SerializeField]
    Transform ListingTransform;

    [SerializeField]
    RoomElement elementPrefab;

    [SerializeField]
    List<RoomElement> listRoomElements;

    private void Start()
    {
        listRoomElements = new List<RoomElement>();
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        CleanListRoomElements();
        foreach(var room in roomList)
        {
            if(room.RemovedFromList)
            {
                continue;
            }

            RoomElement element = Instantiate(elementPrefab, ListingTransform);
            element.SetRoomName(room.Name);

            listRoomElements.Add(element);
        }
    }

    private void CleanListRoomElements()
    {
        foreach(var room in listRoomElements)
        {
            Destroy(room.gameObject);
        }

        listRoomElements.Clear();
    }
}
