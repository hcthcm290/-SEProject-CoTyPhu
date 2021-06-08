using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseMerchant : MonoBehaviour
{
	public string Name
	{
		get { return _name; }
		//set { _name = value; }
	}

	public MerchantTag TagName
	{
		get { return _tagName; }
	}

    public BaseSkill Skill
    {
        get { return _skill; }
    }

    public BaseStatus PassiveSkill
	{
		get { return _passiveSkill; }
		//set { _description = value; }
	}

	public int MaxMana
	{
		get { return _maxMana; }
		//set { _type = value; }
	}

	//  Fields ----------------------------------------
	private string _name;
	public MerchantTag _tagName;
	[SerializeField]
	private BaseSkill _skill;
	private BaseStatus _passiveSkill;
	private int _maxMana;
	public string Story;

	//  Initialization --------------------------------
	public void Set(MerchantTag tagName, string name, BaseSkill skill, BaseStatus passive, int maxMana, string story)
	{
		_tagName = tagName;
		_name = name;
		_skill = skill;
		_passiveSkill = passive;
		_maxMana = maxMana;
		Story = story;
	}
	public void Lock()
    {
		_skill = Instantiate(Skill, this.transform);
    }
	public abstract void Init();
}
