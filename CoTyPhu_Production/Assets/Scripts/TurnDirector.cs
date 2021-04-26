using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class TurnDirector : MonoBehaviourPunCallbacks
{
    public static TurnDirector Ins;
    [SerializeField] List<Player> _listPlayer;
    int _idPlayerTurn = -1;
    Stack<int> _playerTurnExtraPhase;
    List<ITurnListener> _listTurnListener;
    int _count = 0;


    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        if(PhotonNetwork.IsMasterClient)
        {
            foreach(var player in _listPlayer)
            {
                photonView.RPC("CreateNewPlayer", newPlayer, false, player.Id);
            }

            foreach(var player in PhotonNetwork.PlayerList)
            {
                if(player == newPlayer)
                {
                    photonView.RPC("CreateNewPlayer", newPlayer, true, _count);
                }
                else
                {
                    photonView.RPC("CreateNewPlayer", player, false, _count);
                }
            }
            _count++;
        }
    }

    [PunRPC]
    private void CreateNewPlayer(bool isMine, int id)
    {
        PlayerObjectPool playerPool;
        if (PlayerObjectPool.Ins == null)
        {
            playerPool = GameObject.Find("PlayerPool").GetComponent<PlayerObjectPool>();
        }
        else
        {
            playerPool = PlayerObjectPool.Ins;
        }

        if(isMine)
        {
            Player player = playerPool.PlayersPool[0].GetComponent<Player>();
            player.gameObject.SetActive(true);
            player.Id = id;
            _listPlayer.Add(player);
        }
        else
        {
            Player player = playerPool.PlayersPool[1].GetComponent<Player>();
            playerPool.PlayersPool.RemoveAt(1);
            player.gameObject.SetActive(true);
            player.Id = id;
            _listPlayer.Add(player);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Ins = this;
        _playerTurnExtraPhase = new Stack<int>();
    }

    // Update is called once per frame
    void Update()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (_idPlayerTurn == -1 && _listPlayer.Count > 0)
            {
                _idPlayerTurn = 0;
                _listPlayer.Find(x => x.Id == _idPlayerTurn).StartPhase(1);
            }
        }
    }

    /// <summary>
    /// This function is public for test purpose, 
    /// the reason it's public cause it has to be called after player join room, and the call back is in another class
    /// This function may be private and called on Start in real build
    /// main game, they already in a room
    /// </summary>
    public void InitializePlayer()
    {
        // Initialize the player object for all the client in room when game is started
        if (PhotonNetwork.IsMasterClient)
        {
            foreach (var player in PhotonNetwork.PlayerList)
            {
                foreach(var other in PhotonNetwork.PlayerList)
                {
                    if (other.UserId == player.UserId)
                    {
                        photonView.RPC("CreateNewPlayer", player, true, _count);
                    }
                    else
                    {
                        photonView.RPC("CreateNewPlayer", other, false, _count);
                    }
                }

                _count++;
            }
        }
    }

    [PunRPC]
    private void _EndOfPhaseServer()
    {
        if(PhotonNetwork.IsMasterClient)
        {
            _idPlayerTurn = (_idPlayerTurn + 1) % _listPlayer.Count;
            photonView.RPC("_StartPhase", RpcTarget.All, _idPlayerTurn, 1);
        }
    }

    [PunRPC]
    private void _StartPhase(int idPlayer, int phaseID)
    {
        Debug.Log(idPlayer.ToString() + " : " + phaseID);
        _idPlayerTurn = idPlayer;
        _listPlayer.Find(x => x.Id == _idPlayerTurn).StartPhase(phaseID);
    }

    /// <summary>
    /// This function is called by the player to notify the turn director 
    /// it has finished its phase
    /// </summary>
    public void EndOfPhase()
    {
        photonView.RPC("_EndOfPhaseServer", RpcTarget.MasterClient);
    }

    public bool IsMyTurn(int playerID)
    {
        if(_playerTurnExtraPhase.Count != 0)
        {
            if(_playerTurnExtraPhase.Peek() == playerID)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        return _idPlayerTurn == playerID;
    }

    // function to handle Switch to next phase of current player
    private void ToNextPhase()
    {

    }

    public void RequestTurn(int playerID)
    {

    }

    public void SubscribeTurnListener(ITurnListener listener)
    {
        if(_listTurnListener.Contains(listener))
        {
            return;
        }
        else
        {
            _listTurnListener.Add(listener);
        }
    }

    public void UnsubscribeTurnListener(ITurnListener listener)
    {
        if (_listTurnListener.Contains(listener))
        {
            _listTurnListener.Remove(listener);
        }
        else
        {
            return;
        }
    }
}
