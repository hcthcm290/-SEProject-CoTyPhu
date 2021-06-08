using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CreateObject : MonoBehaviour
{
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            GameObject obj = PhotonNetwork.Instantiate("player", transform.position, transform.rotation);
            obj.name = (string)PhotonNetwork.LocalPlayer.CustomProperties["Nickname"];
        }
    }

    private void Start()
    {
        
    }
}
