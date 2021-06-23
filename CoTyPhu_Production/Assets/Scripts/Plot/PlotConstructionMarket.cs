using UnityEngine;

/// <summary>
/// PLOT CONSTRUCTION TEMPLE CONTROL ACTIONS OF "MARKET (A-H)" 
/// </summary>
public class PlotConstructionMarket : PlotConstruction
{
	public int[] baseFee = new int[5];
	//  Properties ------------------------------------
	public int Level 
	{ 
		get => _level; 
		set { _level = value; }
	}
    public override Player Owner { get => base.Owner; 
		set
		{
			base.Owner = value;
			if(Owner != null)
            {
				BuildHouse();
            }
			else if(Owner == null)
            {
				_level = 0;
				Destroy(currentHouse);
			}
		}
	}
	public float UpgradePrice;


	private Transform _buildPoint;
	GameObject currentHouse;
	public static ParticleSystem Firework;
	[SerializeField] ParticleSystem _fireworkPrefab;

	//  Fields ----------------------------------------
	[SerializeField] protected int _level;


	//  Initialization --------------------------------
	public PlotConstructionMarket(PLOT id, string name, string description, int entryFee, int price) 
		: base(id, name, description, entryFee, price){ }


	//  Methods ---------------------------------------
	public int UpgradeFee(int from, int to)
	{
		float sum = 0;
		for (; from < to; from++) sum += UpgradePrice;
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
			_entryFee = baseFee[_level];
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
		_level = level;
		_entryFee = baseFee[_level];
		BuildHouse();
    }

	void BuildHouse()
    {
		var plotHousePool = PlotHousesPool.Ins;

		if (plotHousePool != null)
		{
			var prefab = plotHousePool.GetPrefab(Owner.GetMerchant().TagName, _level);

			if (prefab != null)
			{
				GameObject wrapper = new GameObject();
				wrapper.transform.parent = this.transform;
				wrapper.transform.position = _buildPoint.transform.position;
				wrapper.transform.localScale = Vector3.one;

				GameObject house = Instantiate(prefab, wrapper.transform);
				var currentRotation = house.transform.rotation.eulerAngles;
				var targetRotationY = _buildPoint.transform.rotation.eulerAngles.y;
				house.transform.rotation = Quaternion.Euler(currentRotation.x, targetRotationY, currentRotation.z);

				Destroy(currentHouse);
				currentHouse = wrapper;

				Firework.transform.position = _buildPoint.transform.position;
				Firework.Play();
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

		if(_fireworkPrefab != null && Firework == null)
        {
			Firework = Instantiate(_fireworkPrefab);
        }

	}

    //  Event Handlers --------------------------------
}
