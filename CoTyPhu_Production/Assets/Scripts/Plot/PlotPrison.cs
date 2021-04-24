using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// PLOT.PRISON (8) MANAGE THE PROPERTIES OF PRISON PLOT WHICH BLOCK PLAYER WHEN ENTER
/// </summary>
public class PlotPrison : Plot
{
	//  Events ----------------------------------------


	//  Properties ------------------------------------
	public int ReleaseFeePerRound { get => _releaseFeePerRound; }
	public Dictionary<Player, int> AllPlayerImprisonDurations { get => _playerImprisonDuration; }


	//  Fields ----------------------------------------
	protected int _releaseFeePerRound;
	protected Dictionary<Player, int> _playerImprisonDuration = new Dictionary<Player, int>();


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
		_playerImprisonDuration.Add(player, 1);
	}

	private void Release(Player player)
	{
		_playerImprisonDuration.Remove(player);
	}

	public int GetReleaseFee(Player player)
    {
		return PlayerImprisonDuration(player) * ReleaseFeePerRound;
	}

	public override Action ActionOnEnter(Player obj)
    {
		return new LambdaAction(() =>
		{
			Imprison(obj);
		});
	}

	public override Action ActionOnLeave(Player obj)
	{
		return null;
		//TODO: Check the release condition, if satisfied, Release the player, else increase PlayerImprisonDuration
	}


	//  Event Handlers --------------------------------

}
