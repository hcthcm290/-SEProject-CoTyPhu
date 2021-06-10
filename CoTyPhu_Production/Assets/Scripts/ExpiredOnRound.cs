using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpiredOnRound : MonoBehaviour, ITurnListener
{
	//  Events ----------------------------------------


	//  Properties ------------------------------------
	public BaseStatus Status
	{
		get { return _status; }
		set { _status = value; }
	}

	public int ExpiredRound
	{
		get { return _expiredRound; }
		set { _expiredRound = value; }
	}

	//  Fields ----------------------------------------
	private BaseStatus _status;
	private int _expiredRound;
	private Player _player;
	private bool startToCount = false;

	//  Initialization --------------------------------
	public void Init(BaseStatus status, int expiredRound)
	{
		_status = status;
		_expiredRound = expiredRound;
		_player = TurnDirector.Ins.GetPlayerHaveTurn();
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
		if(idPlayer == _player.Id)
        {
			if(startToCount)
            {
				ExpiredRound -= 1;
				if (ExpiredRound <= 0)
				{
					Status.Remove(true);
				}
			}
        }
        else
        {
			startToCount = true;
        }
	}

	public void OnEndTurn(int idPlayer)
	{

	}


	//  Event Handlers --------------------------------

}