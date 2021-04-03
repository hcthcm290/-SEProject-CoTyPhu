using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;
using Photon.Pun;

public class PlayerElement : MonoBehaviourPunCallbacks 
{
    [SerializeField]
    Player player;

    [SerializeField]
    Text playerName;

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
        playerName.text = (string)player.CustomProperties["Basename"] + "_" + (string)player.CustomProperties["Nickname"];
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        if (targetPlayer.UserId != player.UserId)
            return;

        foreach(var item in changedProps)
        {
            player.CustomProperties[item.Key] = item.Value;    
        }
    }
}
