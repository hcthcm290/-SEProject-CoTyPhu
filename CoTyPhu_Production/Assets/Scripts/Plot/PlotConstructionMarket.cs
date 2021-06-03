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
	GameObject currentHouse;

	//  Fields ----------------------------------------
	[SerializeField] protected int _level;
	protected float[] _upgradeOffset;


	//  Initialization --------------------------------
	public PlotConstructionMarket(PLOT id, string name, string description, int entryFee, int price) 
		: base(id, name, description, entryFee, price){ }


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

    public override IAction ActionOnEnter(Player player)
    {
		NotifyPlotEnter(player);

		if(Owner == null)
        {

        }
		else if(Owner.Id != player.Id)
        {
			player.ChangeMana(2);

			Debug.Log("Pay the rent");
			var entryFee = EntryFee;
			Debug.Log(entryFee);

			// TODO
			// Pay the rent
			Bank.Ins.TransactBetweenPlayers(player, Owner, entryFee);

			NotifyPayPlotFee(player);
		}
		else if(Owner.Id == player.Id)
        {
			player.ChangeMana(1);
		}

		// If this player is in client's control
		if (player.MinePlayer)
        {
			if (Owner == null)
			{
				StopPhaseUI.Ins.Activate(PhaseScreens.PlotBuyUI, this);
			}
			else if (Owner.Id == player.Id)
			{
				// Activate Market Upgrade UI
				StopPhaseUI.Ins.Activate(PhaseScreens.MarketUpgradeUI, this);
			}
			else if (Owner.Id != player.Id)
			{
				// Active Market Rebuy UI
				StopPhaseUI.Ins.Activate(PhaseScreens.PlotRebuyUI, this);
			}
		}
        else
        {

        }
		return null;
    }

	public void Upgrade(int level)
    {
		var plotHousePool = PlotHousesPool.Ins;

		if(plotHousePool != null)
        {
			var prefab = plotHousePool.GetPrefab(level);

			if(prefab != null)
            {
				GameObject house = Instantiate(prefab, transform);
				house.transform.position = _buildPoint.transform.position;
				house.transform.rotation = _buildPoint.transform.rotation;

				Destroy(currentHouse);
				currentHouse = house;

            }
        }
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

		_upgradeOffset = new float[] { 0.15f, 0.15f, 0.15f, 0.15f, 0.15f, };
	}

    //  Event Handlers --------------------------------
}
