using UnityEngine;


/// <summary>
/// PLOT CONSTRUCTION TEMPLE CONTROL ACTIONS OF "ABANDONED ELEMENT TEMPLES" 
/// </summary>
public class PlotConstructionTemple : PlotConstruction
{
	//  Events ----------------------------------------


	//  Properties ------------------------------------
	public float StartBonus { get => _startBonus; }
	public float BonusIncrement { get => _bonusIncrement; }

	//  Fields ----------------------------------------
	protected float _startBonus = 0f;
	protected float _bonusIncrement = 0.5f;


	//  Initialization --------------------------------
	public PlotConstructionTemple(PLOT id, string name, string description, int entryFee, int price) : base(id, name, description, entryFee, price) { }


	//  Methods ---------------------------------------
	protected void IncreasePurchasePrice()
    {
		_startBonus += _bonusIncrement;
    }

    public override IAction ActionOnEnter(Player player)
    {
        if(player.MinePlayer)
        {
            if (Owner == null)
            {
                // TODO
                // Receive 1 mana

                // Activate Temple Buy UI
                StopPhaseUI.Ins.Activate(PhaseScreens.TempleBuyUI, this);
            }
            else if (Owner.Id == player.Id)
            {
                // TODO
                // Receive 2 mana

                TurnDirector.Ins.EndOfPhase();
            }
            else if (Owner.Id != player.Id)
            {
                // TODO
                // Receive 1 mana

                // Pay the temple

                // Active Market Rebuy UI

                TurnDirector.Ins.EndOfPhase();
            }
        }
        else
        {

        }
        
        return null;
    }
    //  Event Handlers --------------------------------
}
