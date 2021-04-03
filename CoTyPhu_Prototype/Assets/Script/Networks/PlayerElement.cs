using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;
using Photon.Pun;

public class PlayerElement : MonoBehaviour
{
    [SerializeField]
    Player player;

    [SerializeField]
    Text playerName;

    [SerializeField]
    Text readyStatus;

    [SerializeField]
    bool ready = false;

    public void SetPlayer(Player player)
    {
        this.player = player;
    }

    public Player GetPlayer()
    {
        return this.player;
    }

    private void Update()
    {
        playerName.text = player.NickName;

        if(ready)
        {
            readyStatus.text = "R";
        }
        else
        {
            readyStatus.text = "";
        }
    }

    public bool GetReadyStatus() { return ready; }

    public void SetReadyStatus(bool value) { this.ready = value; }
}
