using UnityEngine;

/// <summary>
/// PLOT_CONSTRUCTIONS ARE MARKET AND TEMPLE WHICH CAN BE BUILT ON
/// </summary>
public class PlotConstruction: Plot
{
	//  Events ----------------------------------------


	//  Properties ------------------------------------
	public int EntryFee { get => _entryFee; }
	public int Price { get => _price; }
	public Player Owner 
	{ 
		get => _owner; 
		set { _owner = value; } 
	}
	public static float ReBuyOffset { get => _reBuyOffset; }
	public int PurchasePrice { get => Mathf.RoundToInt(_price * _reBuyOffset); }


	//  Fields ----------------------------------------
	protected int _entryFee;
	protected int _price;
	protected Player _owner;
	protected static float _reBuyOffset = 1.5f;


	//  Initialization --------------------------------
	public PlotConstruction(PLOT id, string name, string description, int entryFee, int price) : base(id, name, description)
	{
		this._entryFee = entryFee;
		this._price = price;
		this._owner = null;
	}


	//  Methods ---------------------------------------
	public override IAction ActionOnEnter(Player obj)
    {
		//TODO: Check Owner --> do action based on Owner state
		return null;
    }


    //  Event Handlers --------------------------------
}
