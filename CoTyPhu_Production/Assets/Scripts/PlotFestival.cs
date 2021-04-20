using UnityEngine;

/// <summary>
/// PLOT.FESTIVAL (16) INCREASE THE ENTRY FEE ( BY AN AMOUNT ) OF A CONSTRUCTION PLOT WHEN ENTERED 
/// </summary>
public class PlotFestival : Plot
{
	//  Events ----------------------------------------


	//  Properties ------------------------------------
	public int BonusValue { get => _bonusValue; }


	//  Fields ----------------------------------------
	private int _bonusValue;


	//  Initialization --------------------------------
	public PlotFestival(PLOT id, string name, string description, int bonusValue) : base(id, name, description)
	{
		this._bonusValue = bonusValue;
	}


    //  Methods ---------------------------------------
	private void ApplyFestivalEffect(PLOT plot)
	{
		//TODO: Increase EntryFee of a Construction by _bonusValue
	}

	public override void ActionOnEnter(dynamic obj)
    {
		//TODO: Get Player's selection
		PLOT plot = new PLOT();
		this.ApplyFestivalEffect(plot);
    }


    //  Event Handlers --------------------------------
}
