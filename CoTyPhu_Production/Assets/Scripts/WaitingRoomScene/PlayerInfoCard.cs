using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;

public class PlayerInfoCard : MonoBehaviour
{
    #region UI properties
    [SerializeField] Text playerName;
    #endregion

    public void SetInfo(Photon.Realtime.Player player)
    {
        playerName.text = player.NickName;
    }
}
