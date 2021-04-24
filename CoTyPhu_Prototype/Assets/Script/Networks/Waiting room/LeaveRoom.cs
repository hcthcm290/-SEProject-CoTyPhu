using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class LeaveRoom : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Leave()
    {
        if(PhotonNetwork.InRoom == true)
        {
            PhotonNetwork.LeaveRoom();
            SceneManager._ins.MoveToCreateRoom();
        }
    }
}
