using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// PLOT_CONSTRUCTIONS ARE MARKET AND TEMPLE WHICH CAN BE BUILT ON
/// </summary>
public class PlotConstruction: Plot
{
	#region Status
	[SerializeField] List<IHirePriceChange> _listStatusHirePrice = new List<IHirePriceChange>();
	[SerializeField] List<IBuyBackPriceChange> _listStatusBbPrice = new List<IBuyBackPriceChange>();

	#endregion

	//  Events ----------------------------------------
	List<IPayPlotFeeListener> _payPlotListeners = new List<IPayPlotFeeListener>();

    //  Properties ------------------------------------
    public int EntryFee { get
        {
			int baseEntryFee = _entryFee;
			int deltaFee = 0;
			foreach(var status in _listStatusHirePrice)
            {
				if(status != null)
                {
					deltaFee = (int)status.GethirePriceChange(baseEntryFee, deltaFee);
                }
            }
			return baseEntryFee + deltaFee;
        } }
	public int Price { get => _price; }
	public virtual Player Owner 
	{ 
		get => _owner; 
		set { _owner = value; } 
	}
	public static float ReBuyOffset { get => _reBuyOffset; }
	public int PurchasePrice { 
		get
		{
			if(_owner == null)
            {
				return _price;
            }
			else
            {
				float basePrice = _price * _reBuyOffset;
				if(this is PlotConstructionMarket)
                {
					basePrice = EntryFee * 2;
                }
				if(this is PlotConstructionTemple)
                {
					_price = Mathf.RoundToInt(basePrice);
                }

				float delta = 0;

				List<IBuyBackPriceChange> listBbStatus = new List<IBuyBackPriceChange>(_listStatusBbPrice);
				foreach(var status in listBbStatus)
                {
					if(status != null)
                    {
						delta = status.GetBuyBackPriceChange(basePrice, delta);
                    }
                }

				return Mathf.RoundToInt(basePrice + delta);
			}
		}

	}


	//  Fields ----------------------------------------
	[SerializeField] public int _entryFee;
	[SerializeField] protected int _price;
	[SerializeField] protected Player _owner;
	protected static float _reBuyOffset = 1.5f;


	//  Initialization --------------------------------
	public PlotConstruction(PLOT id, string name, string description, int entryFee, int price) : base(id, name, description)
	{
		this._entryFee = entryFee;
		this._price = price;
		this._owner = null;
	}

    #region Unity methods
    public new void Start()
    {
		base.Start();
		_payPlotListeners = new List<IPayPlotFeeListener>();
		_listStatusHirePrice = new List<IHirePriceChange>();
		_listStatusBbPrice = new List<IBuyBackPriceChange>();
	}
    #endregion

    #region Methods
    //  Methods ---------------------------------------
    public override IAction ActionOnEnter(Player obj)
    {
		//TODO: Check Owner --> do action based on Owner state
		return null;
    }

	public void AddStatus(IHirePriceChange newStatus)
    {
		if(_listStatusHirePrice == null)
        {
			_listStatusHirePrice = new List<IHirePriceChange>();
        }
		if(!_listStatusHirePrice.Contains(newStatus))
        {
			_listStatusHirePrice.Add(newStatus);
        }
    }

	public void AddStatus(IBuyBackPriceChange newStatus)
    {
		if(_listStatusBbPrice == null)
        {
			_listStatusBbPrice = new List<IBuyBackPriceChange>();
		}
		if(!_listStatusBbPrice.Contains(newStatus))
        {
			_listStatusBbPrice.Add(newStatus);
        }
    }

	public void RemoveStatus(IHirePriceChange status)
    {
		_listStatusHirePrice.Remove(status);
    }

	public void RemoveStatus(IBuyBackPriceChange status)
    {
		_listStatusBbPrice.Remove(status);
    }

	public void SubcribePayPlotFee(IPayPlotFeeListener listener)
    {
		if(_payPlotListeners == null)
        {
			_payPlotListeners = new List<IPayPlotFeeListener>();
        }

		if(!_payPlotListeners.Contains(listener))
        {
			_payPlotListeners.Add(listener);
        }
    }

	public void UnsubcribePayPlotFee(IPayPlotFeeListener listener)
    {
		if (_payPlotListeners == null)
		{
			_payPlotListeners = new List<IPayPlotFeeListener>();
		}

		_payPlotListeners.Remove(listener);
	}

	protected void NotifyPayPlotFee(Player player)
    {
		var listeners = new List<IPayPlotFeeListener>(_payPlotListeners);
		foreach(var listener in listeners)
        {
			listener.OnPayPlotFee(player, this);
        }
    }
    #endregion
    //  Event Handlers --------------------------------
}
