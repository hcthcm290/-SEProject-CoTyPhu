using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class BtnRandomNickname : MonoBehaviour
{
    [SerializeField]
    Text text;

    int result;

    // Start is called before the first frame update
    void Start()
    {
        RandomNickname();

        ExitGames.Client.Photon.Hashtable customProp = new ExitGames.Client.Photon.Hashtable();
        customProp["Basename"] = Random.Range(-5, -1).ToString();

        PhotonNetwork.LocalPlayer.SetCustomProperties(customProp);

    }

    // Update is called once per frame
    void Update()
    {
        text.text = result.ToString();
    }

    public void RandomNickname()
    {
        result = Random.Range(0, 99);

        ExitGames.Client.Photon.Hashtable customProp = new ExitGames.Client.Photon.Hashtable();
        customProp["Nickname"] = result.ToString();

        PhotonNetwork.LocalPlayer.SetCustomProperties(customProp);
    }
}
