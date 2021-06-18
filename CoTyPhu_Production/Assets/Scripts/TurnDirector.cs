using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public enum Phase
{
    Dice,
    Move,
    Stop,
    Extra,
    Shop,
}

public class TurnDirector : MonoBehaviourPunCallbacks
{
    #region Field
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
    bool _hasPassPlotStart = false;
    bool _isShopStart = true;
    int _playerReadyCount;
    bool _readied = false;

    // The player id corresponding to this user.
    [SerializeField] int _myPlayer;
    public int MyPlayer
    {
        get
        {
            return _myPlayer;
        }
    }

    public Phase IdPhase { get => _idPhase; set => _idPhase = value; }

    public Dictionary<Phase, string> phaseName = new Dictionary<Phase, string>
    {
        { Phase.Dice, "Dice"},
        { Phase.Move, "Move"},
        { Phase.Stop, "Stop"},
        { Phase.Extra, "Extra"}
    };

    [SerializeField] AvailableMerchants _availableMerchants;
    #endregion

    #region Pun Callback
    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            foreach (var player in _listPlayer)
            {
                photonView.RPC("CreateNewPlayer", newPlayer, false, player.Id);
            }

            foreach (var player in PhotonNetwork.PlayerList)
            {
                if (player == newPlayer)
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
    #endregion


    #region Unity methods
    // Start is called before the first frame update
    void Start()
    {
        Ins = this;
        _playerTurnExtraPhase = new Stack<int>();
    }

    // Update is called once per frame
    void Update()
    {
        //if (PhotonNetwork.IsMasterClient)
        //{
        //    if (_idPlayerTurn == -1 && _listPlayer.Count > 0)
        //    {
        //        _idPlayerTurn = 0;
        //        _idPhase = Phase.Shop;
        //        _listPlayer.Find(x => x.Id == _idPlayerTurn).StartPhase(_idPhase);
        //    }
        //}

        if (!_readied)
        {
            InitializePlayer();
            Ready();
        }
    }
    #endregion

    #region Init game
    /// <summary>
    /// This function is public for test purpose, 
    /// the reason it's public cause it has to be called after player join room, and the call back is in another class
    /// This function may be private and called on Start in real build
    /// main game, they already in a room
    /// </summary>
    public void InitializePlayer()
    {
        // Initialize the player object for all the client in room when game is started
        
        //if (PhotonNetwork.IsMasterClient)
        //{
        //    foreach (var player in PhotonNetwork.PlayerList)
        //    {
        //        foreach (var other in PhotonNetwork.PlayerList)
        //        {
        //            if (other.UserId == player.UserId)
        //            {
        //                photonView.RPC("CreateNewPlayer", player, true, _count);
        //            }
        //            else
        //            {
        //                photonView.RPC("CreateNewPlayer", other, false, _count);
        //            }
        //        }

        //        _count++;
        //    }
        //}

        foreach(var player in PhotonNetwork.PlayerList)
        {
            var playerProperties = player.CustomProperties;
            int? id = playerProperties["id"] as int?;
            int? merchantTag = playerProperties["merchantType"] as int?;

            if (player.UserId == PhotonNetwork.LocalPlayer.UserId)
            {
                CreateNewPlayer(true, (int)id, player.NickName, (MerchantTag)merchantTag);
            }
            else
            {
                CreateNewPlayer(false, (int)id, player.NickName, (MerchantTag)merchantTag);
            }
        }
    }

    private void CreateNewPlayer(bool isMine, int id, string playerName, MerchantTag merchantTag)
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

        BaseMerchant merchantPreb = _availableMerchants.listMerchant.Find(x => x.TagName == merchantTag);

        Player player = null;

        if (isMine)
        {
            player = playerPool.PlayersPool[0].GetComponent<Player>();
            _myPlayer = id;
        }
        else
        {
            player = playerPool.PlayersPool[1].GetComponent<Player>();
            playerPool.PlayersPool.RemoveAt(1);
        }

        player.gameObject.SetActive(true);
        player.LockMerchant(merchantPreb);
        player.Id = id;
        player.Name = playerName;
        _listPlayer.Add(player);
        Bank.Ins.AddPlayer(player);
    }
    
    [PunRPC]
    private void ReadyServer(string clientID)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            _count++;
            if (_count == PhotonNetwork.PlayerList.Length)
            {
                StartGame();
            }
        }
    }

    private void Ready()
    {
        _readied = true;
        photonView.RPC("ReadyServer", RpcTarget.MasterClient, PhotonNetwork.LocalPlayer.UserId);
    }

    private void StartGame()
    {
        if(PhotonNetwork.IsMasterClient)
        {
            _idPlayerTurn = 0;
            _idPhase = Phase.Shop;

            photonView.RPC("_StartPhase", RpcTarget.AllBufferedViaServer, _idPlayerTurn, (int)_idPhase);
        }
    }
    #endregion

    [PunRPC]
    private void _EndOfPhaseServer()
    {
        var nextIdPlayerTurn = _idPlayerTurn;
        var nextIdPhase = IdPhase;

        if (PhotonNetwork.IsMasterClient)
        {
            switch (IdPhase)
            {
                case Phase.Dice:
                    nextIdPhase = Phase.Move;
                    break;
                case Phase.Move:
                    {
                        if(_hasPassPlotStart)
                        {
                            nextIdPhase = Phase.Shop;
                            _hasPassPlotStart = false;
                        }
                        else
                        {
                            nextIdPhase = Phase.Stop;
                        }
                        break;
                    }
                case Phase.Stop:
                    nextIdPhase = Phase.Dice;
                    nextIdPlayerTurn = (_idPlayerTurn + 1) % _listPlayer.Count;
                    while (GetPlayer(nextIdPlayerTurn).HasLost && nextIdPlayerTurn != _idPlayerTurn)
                        nextIdPlayerTurn = (_idPlayerTurn + 1) % _listPlayer.Count;

                    if (WinCondition.WinManager.GetInstance().CheckWinner())
                    {
                        photonView.RPC("InformWinners", RpcTarget.AllBufferedViaServer);
                        return;
                    }

                    break;
                case Phase.Shop:
                    {
                        if(_isShopStart)
                        {
                            nextIdPhase = Phase.Dice;
                            _isShopStart = false;
                        }
                        else
                        {
                            nextIdPhase = Phase.Stop;
                        }
                    }
                    break;
                case Phase.Extra:
                    // TODO
                    // later
                    break;
            }

            photonView.RPC("_StartPhase", RpcTarget.All, nextIdPlayerTurn, (int)nextIdPhase);
        }
    }

    [PunRPC]
    private void _StartPhase(int idPlayer, int phaseID)
    {
        Debug.Log("Start Phase " + ((Phase)phaseID).ToString() + " for player " + idPlayer.ToString());

        _idPlayerTurn = idPlayer;
        _idPhase = (Phase)phaseID;
        if ((Phase)phaseID == Phase.Dice)
        {
            if(_listTurnListener == null)
            {
                _listTurnListener = new List<ITurnListener>();
            }
            List<ITurnListener> listeners = new List<ITurnListener>(_listTurnListener);

            foreach (var listener in listeners)
            {
                listener.OnEndTurn(_idPlayerTurn);
                listener.OnBeginTurn(idPlayer);
            }
        }

        _listPlayer.Find(x => x.Id == _idPlayerTurn).StartPhase(_idPhase);
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
        if (_playerTurnExtraPhase.Count != 0)
        {
            if (_playerTurnExtraPhase.Peek() == playerID)
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

    [PunRPC]
    private void HandlePassPlotStartServer(int idPlayer, string idClient)
    {
        if(IsMyTurn(idPlayer) && IdPhase == Phase.Move)
        {
            _hasPassPlotStart = true;
        }
    }

    public void NotifyPassPlotStart(Player player)
    {
        if(IsMyTurn(player.Id) && IdPhase == Phase.Move)
        {
            photonView.RPC("HandlePassPlotStartServer", RpcTarget.MasterClient, player.Id, PhotonNetwork.LocalPlayer.UserId);
        }
    }
    
    private void NotifyPlayerLoseClient(int id_player)
    {
        Player player = GetPlayer(id_player);

        int playerLostCount = _listPlayer.Count(player => player.HasLost == true);
        player.HasLost = true;
        player.Rank = _listPlayer.Count - playerLostCount;
        player.FinalNetworth = player.CalculateNetworth();
    }

    public void NotifyPlayerLose(int id_player)
    {
        photonView.RPC("NotifyPlayerLoseClient", RpcTarget.AllBufferedViaServer, id_player);
    }

    [PunRPC]
    private void InformWinners()
    {
        var winManager = WinCondition.WinManager.GetInstance();
        if (!winManager.CheckWinner())
        {
            Debug.LogError("Server informs winner, but client can't find winner. Please check for network desync.");
            winManager.EndGame();
        }
        else
            winManager.EndGame();
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
    public Player GetMyPlayer()
    {
        return GetPlayer(MyPlayer);
    }

    //Thang zone
    public Player GetPlayerHaveTurn()
    {
        return _listPlayer.Find(x => x.Id == _idPlayerTurn);
    }

    public int CurrentTurn()
    {
        return _count;
    }

    public Phase GetPhase()
    {
        return _idPhase;
    }
}
