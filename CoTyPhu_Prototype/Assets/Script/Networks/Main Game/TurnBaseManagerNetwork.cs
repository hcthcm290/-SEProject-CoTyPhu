using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class TurnBaseManagerNetwork : MonoBehaviourPun
{
    public static TurnBaseManagerNetwork _ins;

    [SerializeField]
    GameObject playerPreb;
    [SerializeField]
    Vector3 startPosition;

    [SerializeField]
    List<Transform> points;

    [SerializeField]
    GameObject player;

    [SerializeField]
    List<Player> players;

    [SerializeField]
    int currentPlayerIndex;

    [SerializeField]
    float delayGame = 3;

    private void Start()
    {
        _ins = this;
        players = new List<Player>(PhotonNetwork.PlayerList);
        currentPlayerIndex = 0;
    }

    private void OnEnable()
    {
        player = PhotonNetwork.Instantiate("player", startPosition, Quaternion.identity);
        player.GetComponent<PlayerNetwork>().points = points;
    }

    private void Update()
    {
        if(PhotonNetwork.IsMasterClient)
        {
            UpdateForServer();
        }
    }

    void UpdateForServer()
    {
        if(delayGame > 0)
        {
            delayGame -= Time.deltaTime;
            if(delayGame <= 0)
            {
                photonView.RPC("Move", RpcTarget.All, players[currentPlayerIndex], Random.Range(2, 12));
                currentPlayerIndex = (currentPlayerIndex + 1) % players.Count;
            }
        }
    }

    [PunRPC]
    private void Move(Player targetPlayer, int diceResult)
    {
        if(targetPlayer.UserId == PhotonNetwork.LocalPlayer.UserId)
        {
            player.GetComponent<PlayerNetwork>().MoveTo(diceResult);
        }
        else
        {
            Debug.Log("Dice Result: " + diceResult.ToString());
        }
    }

    public void FinishGoTo()
    {
        photonView.RPC("FinishTurn", RpcTarget.MasterClient);
    }

    [PunRPC]
    private void FinishTurn()
    {
        delayGame = 3;
    }
}
