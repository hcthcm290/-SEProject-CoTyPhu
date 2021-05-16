using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseMerchant : MonoBehaviour
{
	public string Name
	{
		get { return _name; }
		//set { _name = value; }
	}

	public string TagName
	{
		get { return _tagName; }
	}

	//public BaseSkill Skill
	//{
	//	get { return _skill; }
	//}

	public BaseStatus PassiveSkill
	{
		get { return _passiveSkill; }
		//set { _description = value; }
	}

	public int MaxMana
	{
		get { return maxMana; }
		//set { _type = value; }
	}

	//  Fields ----------------------------------------
	private string _name;
	static string _tagName;
	//private BaseSkill _skill;
	private BaseStatus _passiveSkill;
	private int maxMana;
	public string story;

	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
