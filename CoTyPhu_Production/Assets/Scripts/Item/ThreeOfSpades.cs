using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThreeOfSpades : BaseItem, ITransactionModifier
{
    #region Base class override

    public override Player Owner
    {
        get
        {
            return base.Owner;
        }
        set
        {
            base.Owner = value;
            StartListen();
        }
    }

    public override bool LoadData()
    {
        Set(
            id: 12,
            name: "Three Of Spades",
            price: 100,
            description: "[passive] when you would pay to the bank\n" +
                "deny that payment",
            type: "Passive"
            );
        /*
        var image = GetComponent<Image>();
        if (image != null)
            image.sprite = Resources.Load<Sprite>("Art/Sprite/Sunnary_Feather");
        //*/
        return true;
    }

    public override bool StartListen()
    {
        Bank.Ins.AddTakeMoneyStatus(this);
        return true;
    }

    public override bool Activate(string activeCase)
    {
        if (activated)
            return false;

        Debug.Log("Activate Three Of Spades.");
        // player active animation

        Bank.Ins.RemoveTakeMoneyStatus(this);

        Owner.RemoveItem(this);

        activated = true;
        return base.Activate(activeCase);
    }

    public bool isActivated(Player player, int baseAmount, bool IsBetweenPlayers)
    {
        return player == Owner && baseAmount != 0 && !IsBetweenPlayers;
    }

    public Tuple<Player, int, int> ModifyTransaction(Player target, int baseAmount, int amount)
    {
        Activate("");

        return new Tuple<Player, int, int>(target, 0, 0);
    }
    #endregion
    #region fields
    [SerializeField] private bool activated = false;
    #endregion

    private void Start()
    {
        LoadData();
    }

    public override List<Transform> TargetLocations()
    {
        return new List<Transform>();
    }
}