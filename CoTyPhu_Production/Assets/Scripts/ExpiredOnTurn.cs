using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpiredOnTurn
{
	//  Events ----------------------------------------


	//  Properties ------------------------------------
	public BaseStatus Status
	{
		get { return _status; }
		set { _status = value; }
	}

	public int ExpiredTurn
	{
		get { return _expiredTurn; }
		set { _expiredTurn = value; }
	}

	//  Fields ----------------------------------------
	private BaseStatus _status;
	private int _expiredTurn;


	//  Initialization --------------------------------
	public ExpiredOnTurn(BaseStatus status, int expiredTurn)
	{

	}


	//  Methods ---------------------------------------
	public bool StartListen()
    {
		//+= RemoveExpiredStatus
		return true;
    }

	public bool RemoveExpiredStatus()
	{
		return true;
	}


	//  Event Handlers --------------------------------

}
