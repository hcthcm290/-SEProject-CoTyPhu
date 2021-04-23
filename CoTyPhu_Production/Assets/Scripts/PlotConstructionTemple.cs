using UnityEngine;


/// <summary>
/// PLOT CONSTRUCTION TEMPLE CONTROL ACTIONS OF "ABANDONED ELEMENT TEMPLES" 
/// </summary>
public class PlotConstructionTemple : PlotConstruction
{
	//  Events ----------------------------------------


	//  Properties ------------------------------------
	public float StartBonus { get => _startBonus; }
	public float BonusIncrement { get => _bonusIncrement; }

	//  Fields ----------------------------------------
	protected float _startBonus = 0f;
	protected float _bonusIncrement = 0.5f;


	//  Initialization --------------------------------
	public PlotConstructionTemple(PLOT id, string name, string description, int entryFee, int price) : base(id, name, description, entryFee, price) { }


	//  Methods ---------------------------------------
	protected void IncreasePurchasePrice()
    {
		_startBonus += _bonusIncrement;
    }


	//  Event Handlers --------------------------------
}
