using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class simpleplayercontrol : MonoBehaviourPun
{
    PhotonView pv;

    private void Start()
    {
        pv = transform.GetComponent<PhotonView>();
    }

    private void Update()
    {
        if(pv != null && pv.IsMine)
        {
            if(Input.GetKeyDown(KeyCode.W))
            {
                var s = transform.position;
                s.x -= 1;
                transform.position = s;
            }
        }
    }
}
