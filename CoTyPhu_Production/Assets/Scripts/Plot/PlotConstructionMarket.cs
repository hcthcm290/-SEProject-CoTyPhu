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
	private Transform _buildPoint;


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

    public override IAction ActionOnEnter(Player obj)
    {
		if(this.Owner == null) // nếu ô đất chưa có chủ sở hữu
        {
			if(obj.MinePlayer)
            {
				// mở bảng yêu cầu mua
				StopPhaseUI.Ins.Activate(PhaseScreens.PlotBuyUI, this);
			}
        }
		return null;
    }

    // Unity Methods ----------------------------------

    public new void Start()
    {
		base.Start();
		_buildPoint = transform.Find("Build Point");

		if(_buildPoint == null)
        {
			Debug.LogError("Plot " + Id.ToString() + "does not have build point as child object");
        }
    }

    //  Event Handlers --------------------------------
}
