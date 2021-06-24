using System.Collections.Generic;
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

    [SerializeField] Transform _buildPoint;
    GameObject currentHouse;

    //  Fields ----------------------------------------
    protected float _startBonus = 0f;
	protected float _bonusIncrement = 0.5f;
    public override Player Owner { get => base.Owner; 
        set
        { 
            base.Owner = value;
            if(Owner == null)
            {
                if(currentHouse != null)
                {
                    Destroy(currentHouse);
                }
            }
            else if(Owner != null)
            {
                BuildHouse();
            }
        }
    }


    //  Initialization --------------------------------
    public PlotConstructionTemple(PLOT id, string name, string description, int entryFee, int price) : base(id, name, description, entryFee, price) { }

	//  Methods ---------------------------------------
	protected void IncreasePurchasePrice()
    {
		_startBonus += _bonusIncrement;
    }

    public override IAction ActionOnEnter(Player player)
    {
        return new LambdaAction(() =>
        {
            NotifyPlotEnter(player);
            if (Owner == null)
            {
                player.ChangeMana(1);
            }
            else if (Owner.Id != player.Id)
            {
                player.ChangeMana(2);

                // TODO
                // Pay the temple
                Bank.Ins.TransactBetweenPlayers(player, Owner, EntryFee);

                NotifyPayPlotFee(player);
            }
            else if (Owner.Id == player.Id)
            {
                player.ChangeMana(1);
            }

            if (player.MinePlayer)
            {
                if (Owner == null)
                {
                    // Activate Temple Buy UI
                    StopPhaseUI.Ins.Activate(PhaseScreens.TempleBuyUI, this);
                }
                else if (Owner.Id == player.Id)
                {
                    base.ActionOnEnter(player).PerformAction();
                }
                else if (Owner.Id != player.Id)
                {
                    // Active Temple Rebuy UI
                    StopPhaseUI.Ins.Activate(PhaseScreens.TempleRebuyUI, this);
                    //base.ActionOnEnter(player).PerformAction();
                }
            }
            else
            {

            }
        });
    }

    void BuildHouse()
    {
        var plotHousePool = PlotHousesPool.Ins;

        if (plotHousePool != null)
        {
            var prefab = plotHousePool.GetPrefab(Owner.GetMerchant().TagName, 0);

            if (prefab != null)
            {
                GameObject wrapper = new GameObject();
                wrapper.transform.parent = this.transform;
                wrapper.transform.position = _buildPoint.transform.position;
                wrapper.transform.localScale = Vector3.one;

                GameObject house = Instantiate(prefab, wrapper.transform);
                var currentRotation = house.transform.rotation.eulerAngles;
                var targetRotationY = _buildPoint.transform.rotation.eulerAngles.y;
                house.transform.rotation = Quaternion.Euler(currentRotation.x, targetRotationY, currentRotation.z);

                if(currentHouse != null)
                {
                    Destroy(currentHouse);
                }
                currentHouse = wrapper;
            }
        }
    }
    //  Event Handlers --------------------------------
}
