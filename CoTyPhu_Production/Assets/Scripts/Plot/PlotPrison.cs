using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// PLOT.PRISON (8) MANAGE THE PROPERTIES OF PRISON PLOT WHICH BLOCK PLAYER WHEN ENTER
/// </summary>
public class PlotPrison : Plot, IDiceListener, ITurnListener
{
	//  Events ----------------------------------------


	//  Properties ------------------------------------
	public int ReleaseFeePerRound { get => _releaseFeePerRound; }
	public Dictionary<Player, int> AllPlayerImprisonDurations { get => _playerImprisonDuration; }
	[SerializeField] PlotManager _plotManager;
	[SerializeField] int _maxDuration = 3;


	//  Fields ----------------------------------------
	[SerializeField] protected int _releaseFeePerRound;
	protected Dictionary<Player, int> _playerImprisonDuration;

	[System.Serializable]
	class ImprisonDuration
    {
		public Player player;
		public int duration;

		public ImprisonDuration(Player p, int d)
        {
			player = p;
			duration = d;
        }
    }
	[SerializeField] List<ImprisonDuration> _listImprisonDuration
    {
        get
        {
			List<ImprisonDuration> l = new List<ImprisonDuration>();
			foreach(var p in _playerImprisonDuration)
            {
				l.Add(new ImprisonDuration(p.Key, p.Value));
            }

			return l;
        }
        set { }
    }


	//  Initialization --------------------------------
	public PlotPrison(PLOT id, string name, string description, int releaseFeePerRound) : base(id, name, description)
	{
		this._releaseFeePerRound = releaseFeePerRound;
	}

	//  Methods ---------------------------------------
	public int PlayerImprisonDuration(Player player)
    {
		if (_playerImprisonDuration.ContainsKey(player))
			return _playerImprisonDuration[player];
		else
			return 0;
	}

	private void Imprison(Player player)
	{
		//add player to playerImprisonDuration
		if(!_playerImprisonDuration.ContainsKey(player))
        {
			_playerImprisonDuration.Add(player, 1);
		}
	}

	private void Release(Player player)
	{
		if(_playerImprisonDuration.ContainsKey(player))
        {
			_playerImprisonDuration.Remove(player);
		}
	}

	public int GetReleaseFee(Player player)
    {
		return PlayerImprisonDuration(player) * ReleaseFeePerRound;
	}

	public override IAction ActionOnEnter(Player obj)
    {
		return new LambdaAction(() =>
		{
			Imprison(obj);

			if(obj.MinePlayer)
            {
				TurnDirector.Ins.EndOfPhase();
            }
		});
	}
    public override IAction ActionOnPass(Player obj)
    {
        return base.ActionOnPass(obj);
    }

    public override IAction ActionOnLeave(Player obj)
	{
		return null;
		//TODO: Check the release condition, if satisfied, Release the player, else increase PlayerImprisonDuration
	}

    private new void Start()
    {
		base.Start();
		_playerImprisonDuration = new Dictionary<Player, int>();
		StartCoroutine(InitEventListener());
	}

	private IEnumerator InitEventListener()
    {
		// Wait for 1 frame to let all game object init its own
		yield return 0;

		Dice.SubscribeDiceListener(this);
		_plotManager.releaseFunc = Release;
		TurnDirector.Ins.SubscribeTurnListener(this);
	}

	private void FreeCardUse(Player _player)
    {
		this.Release(_player);
    }

    

	public bool IsImprisoned(Player player)
    {
		if (player == null)
        {
			Debug.LogError("Cannot check imprison for null player");
			throw new System.Exception("Cannot check imprison for null player");
		}

		if(this.PlayerImprisonDuration(player) == 0)
        {
			return false;
        }
		else
        {
			return true;
        }
    }

	public bool IsImprisoned(int idPlayer)
    {
		Player player = TurnDirector.Ins.GetPlayer(idPlayer);

		if (player == null)
        {
			Debug.LogError("Cannot check imprison with id player null");
			throw new System.Exception("Cannot check imprison with id player null");
		}

		if (this.PlayerImprisonDuration(player) == 0)
		{
			return false;
		}
		else
		{
			return true;
		}
	}

    public int GetDiceListenerPriority()
    {
		return 1;
    }

	//  Event Handlers --------------------------------
	public void OnBeginTurn(int idPlayer)
	{
		Player player = TurnDirector.Ins.GetPlayer(idPlayer);

		if(_playerImprisonDuration.ContainsKey(player))
        {
			_playerImprisonDuration[player]++;
		}
	}

	public void OnEndTurn(int idPlayer)
	{
	}

	public void OnRoll(int idPlayer, List<int> result)
	{
		if (Dice.IsDouble(result))
		{
			Player player = TurnDirector.Ins.GetPlayer(idPlayer);

			this.Release(player);
		}
		else
		{
			Player player = TurnDirector.Ins.GetPlayer(idPlayer);

			
			if (PlayerImprisonDuration(player) == (_maxDuration + 1)) // +1 because when init, it's value = 1
			{
				Bank.Ins.TakeMoney(player, GetReleaseFee(player));

				this.Release(player);
			}
		}
	}
}
