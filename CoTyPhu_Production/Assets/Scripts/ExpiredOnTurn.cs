using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpiredOnTurn :MonoBehaviour, ITurnListener
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
	[SerializeField]
	private BaseStatus _status;
	[SerializeField]
	private int _expiredTurn;


	//  Initialization --------------------------------
	public void Init(BaseStatus status, int expiredTurn)
	{
		Status = status;
		ExpiredTurn = expiredTurn;
		StartListen();
	}


	//  Methods ---------------------------------------
	public bool StartListen()
    {
		TurnDirector.Ins.SubscribeTurnListener(this);
		return true;
    }

	public void OnBeginTurn(int idPlayer)
	{
		ExpiredTurn -= 1;
		if (ExpiredTurn <= 0)
		{
			TurnDirector.Ins.UnsubscribeTurnListener(this);
			Status.Remove(true);
		}
	}

	public void OnEndTurn(int idPlayer)
	{

	}


	//  Event Handlers --------------------------------

}
