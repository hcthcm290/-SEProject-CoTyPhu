using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice
{
    #region Singletone

    private static Dice _ins;

    private Dice()
    {

    }

    public static Dice Ins()
    {
        if (_ins == null)
            _ins = new Dice();

        return _ins;
    }

    #endregion

    List<int> _result;

    int _baseDiceCount;

    List<IDiceListener> _listDiceListener;

    /// <summary>
    /// Roll the dice
    /// </summary>
    /// <param name="idPlayer">player who roll he dice must provide its id</param>
    /// <returns></returns>
    public List<int> Roll(int idPlayer)
    {
        _result.Clear();

        int diceCount = _baseDiceCount;
        
        // Send diceCount to Effects that effect number of dice
        ////////////////////////
        // Code go here

        ////////////////////////

        // Roll the dice
        for(int i = 0; i<diceCount; i++)
        {
            // reason we random up to 7 because the max is exclusive;
            _result.Add(Random.Range(1, 7));
        }

        // Notify the listener
        foreach(var listener in _listDiceListener)
        {
            listener.OnRoll(idPlayer, _result);
        }

        return _result;
    }

    List<int> GetLastResult()
    {
        return _result;
    }

    public void SubscribeDiceListener(IDiceListener listener)
    {
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
}
