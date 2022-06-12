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
	public int LuckyDrawMoney { get => _luckyDrawMoney; }
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
	[SerializeField] private int _luckyDrawMoney = 0;
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
	public void AddMoneyToLuckyDraw(int amount)
    {
		//_moneyBank -= amount;
		_luckyDrawMoney += amount;
    }

	public void TakeLuckyDrawMoney(Player player, int amount)
    {
		int availableAmount = (_luckyDrawMoney < amount) ? _luckyDrawMoney : amount;

		SendMoney(player, availableAmount);
		_luckyDrawMoney -= availableAmount;

	}

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

		_moneyPlayer.Add(player, 2000);
		_moneyPlayers.Add(new PairPlayer() { player = player, money = 2000 });
		GoldChange?.Invoke(player);

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

	public bool TakeMoney(Player player, int amount, bool isBetweenPlayer = false)
	{
		if (!_moneyPlayer.ContainsKey(player)) return false;
		if (player.HasLost) return false;

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
		_moneyBank += amount;
		if (_moneyPlayer[player] < 0)
		{
			Debug.Log("Bank: player lose all money, bankrupt");
			Debug.Log("Bank: sell all your propety");
			foreach (var plotPair in Plot.plotDictionary)
			{
				if (plotPair.Value is PlotConstruction)
				{
					var plot = plotPair.Value as PlotConstruction;
					if (plot.Owner == player)
					{
						SendMoney(player, (int) (plot.PurchasePrice * 0.5f));
						plot.Owner = null;
					}
				}
			}
			if (_moneyPlayer[player] < 0)
			{
				Debug.Log("Bank: player has nothing else to sell, bankrupt");
				player.HasLost = true;
				if (player.MinePlayer)
				{
					Debug.Log("Bank: Notify player lost");
					TurnDirector.Ins.NotifyPlayerLose(player.Id);
				}
			}
		}

		AddGoldNotification(-amount, player);

		GoldChange.Invoke(player);
		SoundManager.Ins.Play(AudioClipEnum.ChaChing);

		return true;
	}

	public void SendMoney(Player player, int amount)
	{
		SendMoney(player, amount, false);
	}

	public void SendMoney(Player player, int amount, bool isBetweenPlayer)
    {
		if (!_moneyPlayer.ContainsKey(player)) return;
		if (player.HasLost) return;

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
		SoundManager.Ins.Play(AudioClipEnum.ChaChing);
	}

	public void TransactBetweenPlayers(Player source, Player destination, int amount)
	{
		if (!_moneyPlayer.ContainsKey(source) || !_moneyPlayer.ContainsKey(destination) || source == destination) return;

		if (TakeMoney(source, amount, true))
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
	int MoneyAmount
    {
		get;
    }
	// object Source and Destination may be a List
	object Source
    {
		get;
    }
	object Destination
    {
		get;
    }
}