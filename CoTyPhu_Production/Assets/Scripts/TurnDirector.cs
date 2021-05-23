using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public enum Phase
{
    Dice,
    Move,
    Stop,
    Extra,
}

public class TurnDirector : MonoBehaviourPunCallbacks
{
    public static TurnDirector Ins;
    [SerializeField] List<Player> _listPlayer;
    public List<Player> ListPlayer
    {
        get
        {
            return _listPlayer;
        }
    }
    int _idPlayerTurn = -1;
    Phase _idPhase;
    Stack<int> _playerTurnExtraPhase;
    List<ITurnListener> _listTurnListener;
    int _count = 0;

    // The player id corresponding to this user.
    [SerializeField] int _myPlayer;
    public int MyPlayer
    {
        get
        {
            return _myPlayer;
        }
    }

    public Dictionary<Phase, string> phaseName = new Dictionary<Phase, string>
    {
        { Phase.Dice, "Dice"},
        { Phase.Move, "Move"},
        { Phase.Stop, "Stop"},
        { Phase.Extra, "Extra"}
    };

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

    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();

        InitializePlayer();
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
            _myPlayer = id;
            Bank.Ins.AddPlayer(player);
        }
        else
        {
            Player player = playerPool.PlayersPool[1].GetComponent<Player>();
            playerPool.PlayersPool.RemoveAt(1);
            player.gameObject.SetActive(true);
            player.Id = id;
            _listPlayer.Add(player);
            Bank.Ins.AddPlayer(player);
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
                _idPhase = Phase.Dice;
                _listPlayer.Find(x => x.Id == _idPlayerTurn).StartPhase(_idPhase);
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
            switch (_idPhase)
            {
                case Phase.Dice:
                    _idPhase = Phase.Move;
                    break;
                case Phase.Move:
                    _idPhase = Phase.Stop;
                    break;
                case Phase.Stop:
                    _idPhase = Phase.Dice;
                    _idPlayerTurn = (_idPlayerTurn + 1) % _listPlayer.Count;
                    break;
                case Phase.Extra:
                    // TODO
                    // later
                    break;
            }

            photonView.RPC("_StartPhase", RpcTarget.All, _idPlayerTurn, (int)_idPhase);
        }
    }

    [PunRPC]
    private void _StartPhase(int idPlayer, int phaseID)
    {
        if((Phase)phaseID == Phase.Dice)
        {
            foreach(var listener in _listTurnListener)
            {
                listener.OnEndTurn(_idPlayerTurn);
                listener.OnBeginTurn(idPlayer);
            }
        }

        _idPlayerTurn = idPlayer;
        Phase phase = (Phase)phaseID;
        _listPlayer.Find(x => x.Id == _idPlayerTurn).StartPhase(phase);
    }

    /// <summary>
    /// This function is called by the player to notify the turn director 
    /// it has finished its phase
    /// </summary>
    public void EndOfPhase()
    {
        photonView.RPC("_EndOfPhaseServer", RpcTarget.MasterClient);
    }
    public bool IsMyTurn()
    {
        return IsMyTurn(_myPlayer);
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
        if (_listTurnListener == null) _listTurnListener = new List<ITurnListener>();
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

    public Player GetPlayer(int playerID)
    {
        return _listPlayer.Find(x => x.Id == playerID);
    }
}
