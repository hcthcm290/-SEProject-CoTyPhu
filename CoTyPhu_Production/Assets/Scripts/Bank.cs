using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System;

/// <summary>
/// THE BANK CONTROLS ALL MONEYS FLOW IN GAME
/// </summary>
public class Bank: MonoBehaviour
{
	#region Status
	List<ITransactionModifier> _listMoneyReceiveModify;
	List<ITransactionModifier> _listMoneyTakeModify;


	#endregion

	//  Events ----------------------------------------

	#region Singleton
	private static Bank _ins;
	public static Bank Ins
    {
		get { return _ins; }
    }
    #endregion

    #region Properties
    public int MoneyBank { get => _moneyBank; }
	public Dictionary<Player,int> AllMoneyPlayers { get => _moneyPlayer; }
	
	[System.Serializable]
	public class PairPlayer
    {
		public Player player;
		public int money;
    }
	public List<PairPlayer> _moneyPlayers = new List<PairPlayer>();
    #endregion

    #region Fields
    private int _moneyBank = 10000;
	[SerializeField] private Dictionary<Player, int> _moneyPlayer = new Dictionary<Player, int>();
    #endregion


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

	#region Unity methods
	//  Unity Methods ---------------------------------
	private void Start()
    {
		_ins = this;
		if(_listMoneyReceiveModify == null)
        {
			_listMoneyReceiveModify = new List<ITransactionModifier>();
		}
	}
    #endregion


    #region Methods
    public int MoneyPlayer(Player player)
    {
		if (_moneyPlayer.ContainsKey(player))
			return _moneyPlayer[player];
		else 
			return -65536;
    }

	public void AddPlayer(Player player)
    {
		if(_moneyPlayer.ContainsKey(player))
        {
			Debug.LogError("Player added duplicated in Bank.AddPlayer()");
        }

		_moneyPlayer.Add(player, 6000);
		_moneyPlayers.Add(new PairPlayer() { player = player, money = 6000 });

		if(player.GetMerchant().TagName == MerchantTag.Birger)
        {
			Bank.Ins.SendMoney(player, 600, false);
		}
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

	public void TakeMoney(Player player, int amount, bool isBetweenPlayer = false)
	{
		if (!_moneyPlayer.ContainsKey(player)) return;

		int baseAmount = amount;
		int delta = 0;

		if (_listMoneyTakeModify != null)
		{
			List<ITransactionModifier> ListModifier = new List<ITransactionModifier>(_listMoneyTakeModify);

			foreach (var transactionModifier in ListModifier)
			{
				if (transactionModifier == null) continue;

				if (transactionModifier.isActivated(player, baseAmount, isBetweenPlayer))
				{
					var result = transactionModifier.ModifyTransaction(player, baseAmount, delta);

					(transactionModifier as BaseStatus)?.ExcuteAction();

					player = result.Item1;
					baseAmount = result.Item2;
					delta = result.Item3;
				}
			}
		}

		amount = baseAmount + delta;

		_moneyPlayer[player] -= amount;
		_moneyPlayers.Find(x => x.player == player).money -= amount;
		if (_moneyPlayer[player] <= 0)
		{
			TurnDirector.Ins.NotifyPlayerLose(player.Id);
		}

		AddGoldNotification(amount, player);

		GoldChange.Invoke(player);
	}

	public void SendMoney(Player player, int amount)
	{
		SendMoney(player, amount, false);
	}

	public void SendMoney(Player player, int amount, bool isBetweenPlayer)
    {
		if (!_moneyPlayer.ContainsKey(player)) return;

		int baseAmount = amount;
		int delta = 0;

		if (_listMoneyReceiveModify != null)
		{
			List<ITransactionModifier> ListModifier = new List<ITransactionModifier>(_listMoneyReceiveModify);

			foreach (var transactionModifier in ListModifier)
			{
				if (transactionModifier == null) continue;

				if (transactionModifier.isActivated(player, baseAmount, isBetweenPlayer))
				{
					var result = transactionModifier.ModifyTransaction(player, baseAmount, delta);

					(transactionModifier as BaseStatus)?.ExcuteAction();

					player = result.Item1;
					baseAmount = result.Item2;
					delta = result.Item3;
				}
			}
		}

		amount = baseAmount + delta;

		_moneyPlayer[player] += amount;
		_moneyPlayers.Find(x => x.player == player).money += amount;
		_moneyBank -= amount;

		AddGoldNotification(amount, player);
		GoldChange.Invoke(player);
	}

	public void TransactBetweenPlayers(Player source, Player destination, int amount)
	{
		if (!_moneyPlayer.ContainsKey(source) || !_moneyPlayer.ContainsKey(destination) || source == destination) return;

		TakeMoney(source, amount, true);
		SendMoney(destination, amount, true);
	}

	public void AddReceiveMoneyStatus(ITransactionModifier transactionModifier)
    {
		if(_listMoneyReceiveModify == null)
        {
			_listMoneyReceiveModify = new List<ITransactionModifier>();
        }
		if(!_listMoneyReceiveModify.Contains(transactionModifier))
        {
			_listMoneyReceiveModify.Add(transactionModifier);
        }
    }

	public void RemoveReceiveMoneyStatus(ITransactionModifier transactionModifier)
    {
		if (_listMoneyReceiveModify == null)
		{
			_listMoneyReceiveModify = new List<ITransactionModifier>();
		}
		_listMoneyReceiveModify.Remove(transactionModifier);
	}

	public void AddTakeMoneyStatus(ITransactionModifier transactionModifier)
	{
		if (_listMoneyTakeModify == null)
		{
			_listMoneyTakeModify = new List<ITransactionModifier>();
		}
		if (!_listMoneyTakeModify.Contains(transactionModifier))
		{
			_listMoneyTakeModify.Add(transactionModifier);
		}
	}

	public void RemoveTakeMoneyStatus(ITransactionModifier transactionModifier)
	{
		if (_listMoneyTakeModify == null)
		{
			_listMoneyTakeModify = new List<ITransactionModifier>();
		}
		_listMoneyTakeModify.Remove(transactionModifier);
	}
	#endregion

	#region Thang code for Notification
	public void AddGoldNotification(int gold, Player p)
    {
		FloatingNotificationManager.Ins.AddGoldNotification(gold, p);
    }
	#endregion

	//  Event Handlers --------------------------------
	public delegate void GoldChangeHandler(Player p);
	public event GoldChangeHandler GoldChange;
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