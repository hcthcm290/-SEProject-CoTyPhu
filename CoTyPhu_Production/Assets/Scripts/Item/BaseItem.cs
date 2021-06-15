using System.Collections.Generic;
using UnityEngine;

//  Class Attributes ----------------------------------

/// <summary>
/// Base class for item
/// </summary>
[System.Serializable]
public abstract class BaseItem : MonoBehaviour
{
	static public System.Random rng = new System.Random();
	//  Events ----------------------------------------
	public delegate void ItemActivateHandler(int id);
	public event ItemActivateHandler ItemActivate;

	public delegate void ItemDestroyHandeler();
	public event ItemDestroyHandeler ItemDestroy;

	//  Properties ------------------------------------

	public int Id
    {
        get { return _id; }
        //set { _id = value; }
    }

	public string Name
	{
		get { return _name; }
		//set { _name = value; }
	}

	public int Price
	{
		get { return _price; }
		set { _price = value; }
	}

	public string Description
	{
		get { return _description; }
		//set { _description = value; }
	}

	public string Type
	{
		get { return _type; }
		//set { _type = value; }
	}

	public virtual bool CanActivate
    {
		get { return _canActivate; }
        set { _canActivate = value; }
    }

	public virtual Player Owner
    {
		get { return _owner; }
		set { _owner = value; }
    }

	//  Fields ----------------------------------------
	[SerializeField] protected int _id;
	[SerializeField] protected string _name;
	[SerializeField] protected int _price;
	[SerializeField] protected string _description;
	[SerializeField] protected string _type;

	[SerializeField] protected bool _canActivate;

	[SerializeField] private Player _owner;

	//  Initialization --------------------------------
	public void Set(int id, string name, int price, string description, string type)
    {
		_id = id;
		_name = name;
		_price = price;
		_description = description;
		_type = type;
	}

	//  Methods ---------------------------------------
	public abstract bool LoadData();

	public abstract bool StartListen();

	public virtual bool Activate(string activeCase)
    {
		if(ItemActivate != null)
			ItemActivate.Invoke(Id);

		MoveToCenter(new LambdaAction(() =>
		{
			List<Transform> targets = TargetLocations();

			var prefab = PrefabContainer.Ins.magicOrb;

			foreach (var target in targets)
            {
				var orb = Instantiate(prefab);
				Vector3 targetPos = orb.targetPos = target.position;
				Vector3 pivotOffset = new Vector3(rng.Next(0,15), rng.Next(0, 15), rng.Next(0, 15));
				Vector3 initialPos = orb.transform.position;
				orb.pivotPos = initialPos - pivotOffset;
				orb.MaxTime = Vector3.Angle(pivotOffset, targetPos - initialPos) / 180 + 1;
				orb.HasStart = true;
            }
		}));

		return true;
    }

	public virtual bool Remove(bool triggerEvent)
	{
		if(triggerEvent)
			if (ItemDestroy != null)
				ItemDestroy.Invoke();
		return true;
	}

	//  Event Handlers --------------------------------


	//  Splash effect on use
	public void MoveToCenter(IAction onComplete = null)
    {
		if (Owner != null)
			transform.parent = UIPlayerBox.UILocation[Owner].transform.Find("PlayerBox/MerchantImage");
		else return;

		// TODO: Add glow

		MoveStraightEvenly moveComponent = gameObject.AddComponent<MoveStraightEvenly>();
		if (onComplete != null)
			moveComponent.ListenTargetReached(onComplete);
		moveComponent.Target = new Vector3(0, 0, 0);
    }

	protected List<Transform> InvolvedLocations = new List<Transform>();
	public virtual List<Transform> TargetLocations()
    {
		List<Transform> ans = InvolvedLocations;

		if (this is IPlotChooserAction)
        {
			PLOT? plot = (this as IPlotChooserAction).plot;
			if (plot != null)
				ans.Add(Plot.plotDictionary[plot.Value].transform);
        }

		if (this is IPayPlotFeeListener)
        {
			PLOT? plot = (this as IPayPlotFeeListener).AssignedPlot;
			if (plot != null)
				ans.Add(Plot.plotDictionary[plot.Value].transform);
		}

		// If there is no Target Location found,
		// Assume Item is Self-buff, and target the Owner
		if (ans.Count == 0 && Owner != null)
			ans.Add(UIPlayerBox.UILocation[Owner].transform.Find("PlayerBox/MerchantImage"));
		
		return ans;
    }
}

