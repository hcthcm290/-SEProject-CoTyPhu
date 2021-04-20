using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnDirector : MonoBehaviour
{
    [SerializeField] List<Player> _listPlayer;
    int _idPlayerTurn;
    Stack<int> _playerTurnExtraPhase;
    List<ITurnListener> _listTurnListener;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// This function is called by the player to notify the turn director 
    /// it has finished its phase
    /// </summary>
    public void EndOfPhase()
    {

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
