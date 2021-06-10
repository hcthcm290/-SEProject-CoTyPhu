using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//skill sẽ tồn tại 2 loại, một data gốc thuộc về marchant, một là skill mà player có thể kích hoạt được, có thể thay đổi lượng mana cost hiện tại tuỳ theo status

public abstract class BaseSkill : MonoBehaviour
{
	public string Name
	{
		get { return _name; }
		//set { _name = value; }
	}

	public int ManaCost
	{
		get { return _manaCost; }
	}

	public int CurrentManaCost
	{
		get { return _currentManaCost; }
		set { _currentManaCost = value; }
	}

	public string Description
	{
		get { return _description; }
		//set { _description = value; }
	}

	//  Fields ----------------------------------------
	private string _name;
	private int _manaCost;
	private int _currentManaCost;
	private string _description;

	public Player Owner;

	//  Initialization --------------------------------
	public void Set(string name, int manacost, int currentmanacost, string description)
	{
		_name = name;
		_manaCost = manacost;
		_currentManaCost = currentmanacost;
		_description = description;
	}

	//  Methods ---------------------------------------
	public virtual bool CanActivate()
	{
		return false;
	}

    public void ResetSkill()
    {
		_currentManaCost = _manaCost;
        //reset skill after activate to make sure buff will expire (if it need to be).
    }

    public virtual bool Activate()
    {
		Owner.ChangeMana(-ManaCost);
		if (Owner.GetMana() >= 0)
			return true;
		else
			return false;
    }
}
