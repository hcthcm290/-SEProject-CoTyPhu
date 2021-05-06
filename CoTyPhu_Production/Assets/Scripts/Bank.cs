using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

/// <summary>
/// THE BANK CONTROLS ALL MONEYS FLOW IN GAME
/// </summary>
public class Bank: MonoBehaviour
{
	//  Events ----------------------------------------

	//	Singleton -------------------------------------
	private static Bank _ins;
	public static Bank Ins
    {
		get { return _ins; }
    }

	//  Properties ------------------------------------
	public int MoneyBank { get => _moneyBank; }
	public Dictionary<Player,int> AllMoneyPlayers { get => _moneyPlayer; }

	//  Fields ----------------------------------------
	private int _moneyBank;
	[SerializeField] private Dictionary<Player, int> _moneyPlayer = new Dictionary<Player, int>();


	//  Initialization --------------------------------
	public Bank()
    {

    }

	public Bank(int moneyBank, Player[] arrPlayers)
	{
		_moneyBank = moneyBank;
		foreach (Player player in arrPlayers)
        {
			AddPlayer(player);
        }
	}

    //  Unity Methods ---------------------------------
    private void Start()
    {
		_ins = this;
    }


    //  Methods ---------------------------------------
    public int MoneyPlayer(Player player)
    {
		if (_moneyPlayer.ContainsKey(player))
			return _moneyPlayer[player];
		else 
			return -65536;
    }

	public void AddPlayer(Player player)
    {
		_moneyPlayer.Add(player, 130);
	}

	public void RemovePlayer(Player player)
	{
		//Take all money of the player and remove him
		if (_moneyPlayer.ContainsKey(player))
        {
			TakeMoney(player, MoneyPlayer(player));
			_moneyPlayer.Remove(player);
		}
	}

	public void TakeMoney(Player player, int amount)
    {
		if (!_moneyPlayer.ContainsKey(player)) return;

		_moneyPlayer[player] -= amount;
		if (_moneyPlayer[player] <= 0)
        {
			//TODO: Bankrupt the player
        }
	}

	public void SendMoney(Player player, int amount)
	{
		if (!_moneyPlayer.ContainsKey(player)) return;

		_moneyPlayer[player] += amount;
		_moneyBank -= amount;
	}

	public void TransactBetweenPlayers(Player source, Player destination, int amount)
	{
		if (!_moneyPlayer.ContainsKey(source) || !_moneyPlayer.ContainsKey(destination) || source == destination) return;

		TakeMoney(source, amount);
		SendMoney(destination, amount);
	}

	//  Event Handlers --------------------------------
}

public interface ITransaction
{
	public int MoneyAmount
    {
		get;
    }
	// object Source and Destination may be a List
	public object Source
    {
		get;
    }
	public object Destination
    {
		get;
    }
}