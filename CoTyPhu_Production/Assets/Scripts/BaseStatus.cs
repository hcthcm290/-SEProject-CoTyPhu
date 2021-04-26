using System;
using UnityEngine;

//  Class Attributes ----------------------------------

/// <summary>
/// Base class for item
/// các class status khi khởi tạo sẽ kế thừa baseStatus và các interface nó cần
/// các hàm cần gọi thuộc tính từ status kiếm tra status có interface nào trước khi gọi hàm Get
/// </summary>
public abstract class BaseStatus: MonoBehaviour
{
	//  Events ----------------------------------------


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

	public string Description
	{
		get { return _description; }
		//set { _description = value; }
	}

	public bool IsConditional
	{
		get { return _isConditional; }
		//set { _isConditional = value; }
	}

	//  Fields ----------------------------------------
	protected int _id;
	protected string _name;
	protected string _description;
	protected bool _isConditional;

	//  Initialization --------------------------------
	public BaseStatus()
	{

	}

	//  Methods ---------------------------------------
	public abstract bool LoadData();

	public abstract bool StartListen();

	//public abstract bool Apply();

	public abstract bool ExcuteAction();

	public virtual bool Remove(bool triggerEvent)
	{
		if (triggerEvent)
			if (StatusDestroy != null)
				StatusDestroy.Invoke();
		return true;
	}

	//  Event Handlers --------------------------------
	public delegate void StatusDestroyHandler();
	public event StatusDestroyHandler StatusDestroy;
}
