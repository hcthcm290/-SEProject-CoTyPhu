using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Dice: MonoBehaviourPunCallbacks
{
    #region Singletone

    private static Dice _ins;

    private Dice()
    {

    }

    public static Dice Ins()
    {
        if (_ins == null)
            Debug.LogError("Dice Object not loaded");

        return _ins;
    }

    private void Start()
    {
        _ins = this;

        PhotonNetwork.NickName = Random.Range(0, 999999).ToString();

        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.SendRate = 40;
        PhotonNetwork.SerializationRate = 40;

        RoomOptions options = new RoomOptions();
        options.MaxPlayers = 5;
        options.PlayerTtl = 0;
        options.PublishUserId = true;

        if (PhotonNetwork.JoinOrCreateRoom("HCTHCM290", options, TypedLobby.Default) == false)
        {
            Debug.LogError("Cannot create or join room");
        }

        TurnDirector.Ins.InitializePlayer();
    }

    public override void OnJoinedRoom()
    {
        TurnDirector.Ins.InitializePlayer();
    }

    #endregion

    List<int> _result;

    int _baseDiceCount = 2;
    int _resultCount;

    List<IDiceListener> _listDiceListener;

    [SerializeField] DiceUI _dicePrefab;
    List<DiceUI> _currentDices;

    [SerializeField] Transform diceSpawnPosition;

    [PunRPC]
    private void _RollServer(int idPlayer)
    {
        int diceCount = _baseDiceCount;

        // Send diceCount to Effects that effect number of dice
        ////////////////////////
        // Code go here

        ////////////////////////

        if (_result == null)
            _result = new List<int>();

        if (_currentDices == null)
        {
            _currentDices = new List<DiceUI>();
        }

        _result.Clear();
        _resultCount = 0;

        // Add or destroy dice object to match dice count
        if (diceCount > _currentDices.Count)
        {
            while (diceCount > _currentDices.Count)
            {
                DiceUI dice = PhotonNetwork.Instantiate("Dice", diceSpawnPosition.position, Quaternion.identity).GetComponent<DiceUI>();
                //DiceUI dice = Instantiate(_dicePrefab, new Vector3(0, 0, 0), Quaternion.identity);
                dice.ReceiveResult += (int result) => {
                    _result.Add(result);
                    _resultCount++;

                    if (_resultCount == diceCount)
                    {
                        photonView.RPC("_ReceiveRollResult", RpcTarget.All, idPlayer, (object)(_result.ToArray()));
                    }
                };
                _currentDices.Add(dice);
            }
        }
        else if (diceCount < _currentDices.Count)
        {
            while (diceCount < _currentDices.Count)
            {
                DiceUI dice = _currentDices[_currentDices.Count - 1];
                PhotonNetwork.Destroy(dice.gameObject);

                _currentDices.RemoveAt(_currentDices.Count - 1);
            }
        }

        for (int i = 0; i < diceCount; i++)
        {
            _currentDices[i].Roll();
        }
    }

    [PunRPC]
    private void _ReceiveRollResult(int idPlayer, object result)
    {
        _result = new List<int>(result as int[]);
        foreach (var listener in _listDiceListener)
        {
            listener.OnRoll(idPlayer, _result);
        }
    }

    /// <summary>
    /// Roll the dice
    /// </summary>
    /// <param name="idPlayer">player who roll he dice must provide its id</param>
    /// <returns></returns>
    public void Roll(int idPlayer)
    {
        photonView.RPC("_RollServer", RpcTarget.MasterClient, idPlayer);
    }

    public void CheatRoll(int idPlayer, int result)
    {
        List<int> results = new List<int>();
        results.Add(result);
        photonView.RPC("_ReceiveRollResult", RpcTarget.All, idPlayer, (object)(results.ToArray()));
    }

    public List<int> GetLastResult()
    {
        return _result;
    }

    public void SubscribeDiceListener(IDiceListener listener)
    {
        if(_listDiceListener == null)
        {
            _listDiceListener = new List<IDiceListener>();
        }
        if (_listDiceListener.Contains(listener))
        {
            return;
        }
        else
        {
            _listDiceListener.Add(listener);
        }
    }

    public void UnsubscribeDiceListener(IDiceListener listener)
    {
        if (_listDiceListener.Contains(listener))
        {
            _listDiceListener.Remove(listener);
        }
        else
        {
            return;
        }
    }

    private void Update()
    {
        
    }

    #region Unused Legacy Code
    [PunRPC]
    private void _RollClient(int idPlayer, int diceCount, object in_torques)
    {
        if (_result == null)
            _result = new List<int>();

        if (_currentDices == null)
        {
            _currentDices = new List<DiceUI>();
        }

        _result.Clear();
        _resultCount = 0;

        Vector3[] torques = in_torques as Vector3[];

        for (int i = 0; i < diceCount; i++)
        {
            _result.Add(0);
        }

        // Add or destroy dice object to match dice count
        if (diceCount > _currentDices.Count)
        {
            while (diceCount > _currentDices.Count)
            {
                DiceUI dice = Instantiate(_dicePrefab, new Vector3(0, 0, 0), Quaternion.identity);
                dice.ReceiveResult += (int result) =>
                {
                    _result[_resultCount] = result;
                    _resultCount++;

                    if (_resultCount == diceCount)
                    {
                        foreach (var listener in _listDiceListener)
                        {
                            listener.OnRoll(idPlayer, _result);
                        }
                    }
                };
                _currentDices.Add(dice);
            }
        }
        else if (diceCount < _currentDices.Count)
        {
            while (diceCount < _currentDices.Count)
            {
                DiceUI dice = _currentDices[_currentDices.Count - 1];
                Destroy(dice.gameObject);

                _currentDices.RemoveAt(_currentDices.Count - 1);
            }
        }

        for (int i = 0; i < diceCount; i++)
        {
            _currentDices[i].Roll();
        }
    }
    #endregion
}
