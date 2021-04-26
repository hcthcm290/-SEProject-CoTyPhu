using UnityEngine;

/// <summary>
/// PLOT CONSTRUCTION TEMPLE CONTROL ACTIONS OF "MARKET (A-H)" 
/// </summary>
public class PlotConstructionMarket : PlotConstruction
{
	//  Events ----------------------------------------


	//  Properties ------------------------------------
	public int Level 
	{ 
		get => _level; 
		set { _level = value; }
	}
	public float[] UpgradeOffset { get => _upgradeOffset; }


	//  Fields ----------------------------------------
	protected int _level;
	protected float[] _upgradeOffset = { 0.15f, };


	//  Initialization --------------------------------
	public PlotConstructionMarket(PLOT id, string name, string description, int entryFee, int price) : base(id, name, description, entryFee, price) { }


	//  Methods ---------------------------------------
	public int UpgradeFee(int from, int to)
	{
		float sum = 0;
		for (; from < to; from++) sum += _upgradeOffset[from] * _price;
		return Mathf.RoundToInt(sum);
	}

	public bool CanBuy(Player player) 
    {
		if (this._owner == player) return false;
		else return false;
		//TODO: check the player money, return true or false if enough
	}

	//  Event Handlers --------------------------------
}
