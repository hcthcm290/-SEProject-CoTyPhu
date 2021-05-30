using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpiredOnTurn :MonoBehaviour
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
		Status = status;
		ExpiredTurn = expiredTurn;
	}


	//  Methods ---------------------------------------
	public bool StartListen()
    {
		//TurnDirector.Ins.OnTurnBegin = += RemoveExpiredStatusCountDown();
		return true;
    }

	public bool RemoveExpiredStatusCountDown()
	{
		ExpiredTurn -= 1;
		if(ExpiredTurn <= 0)
        {
			Status.Remove(true);
        }
		return true;
	}


	//  Event Handlers --------------------------------

}
