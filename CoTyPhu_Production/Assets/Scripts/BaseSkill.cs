using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseSkill : MonoBehaviour
{
	public string Name
	{
		get { return _name; }
		//set { _name = value; }
	}

	public int ManaCost
	{
		get { return _manaCost; }
		//set { _id = value; }
	}

	public int CurrentManaCost
	{
		get { return _currentManaCost; }
		//set { _id = value; }
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

	public bool CanActivate()
	{
		return false;
	}


	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
