using UnityEngine;

//  Class Attributes ----------------------------------

/// <summary>
/// Base class for item
/// </summary>
[System.Serializable]
public abstract class BaseItem : MonoBehaviour
{
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

	public bool CanActivate
    {
		get { return _canActivate; }
        set { _canActivate = value; }
    }

	public Player Owner
    {
		get { return _owner; }
		set { _owner = value; }
    }

	//  Fields ----------------------------------------
	[SerializeField]private int _id;
	[SerializeField]private string _name;
	[SerializeField]private int _price;
	[SerializeField]private string _description;
	[SerializeField]private string _type;

	private bool _canActivate;

	private Player _owner;

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
}

