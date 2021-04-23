using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// THE BANK CONTROLS ALL MONEYS FLOW IN GAME
/// </summary>
public class Bank
{
	//  Events ----------------------------------------


	//  Properties ------------------------------------
	public int MoneyBank { get => _moneyBank; }
	public Dictionary<Player,int> AllMoneyPlayers { get => _moneyPlayer; }

	//  Fields ----------------------------------------
	private int _moneyBank;
	private Dictionary<Player, int> _moneyPlayer = new Dictionary<Player, int>();


	//  Initialization --------------------------------
	public Bank(int moneyBank, Player[] arrPlayers)
	{
		_moneyBank = moneyBank;
		foreach (Player player in arrPlayers)
        {
			AddPlayer(player);
        }
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
		_moneyPlayer.Add(player, 0);
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
	}

	public void TransactBetweenPlayers(Player player1, Player player2, int amount)
	{
		if (!_moneyPlayer.ContainsKey(player1) || !_moneyPlayer.ContainsKey(player2) || player1 == player2) return;

		TakeMoney(player1, amount);
		SendMoney(player2, amount);
	}

	//  Event Handlers --------------------------------
}