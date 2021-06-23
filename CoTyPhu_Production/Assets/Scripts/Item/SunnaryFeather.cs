using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SunnaryFeather : SunnaryItem, IPlotPassByListener
{
    public const int maxItemGrant = 4;
    #region Base class override
    public SunnaryWings wingsPrefab;

    public override Player Owner
    {
        get
        {
            return base.Owner;
        }
        set
        {
            StartListen();
            base.Owner = value;
        }
    }

    public override bool LoadData()
    {
        Set(
            id: 8,
            name: "Feather Of Sunnari",
            price: 700,
            description: "[passive] in this turn, if you pass world tree: \n"+
                "• You gain 1 power stack.\n" +
                "• You get a Wings of sunnari(max 4) for each “shining” effect you have used.\n" +
                "This is considered as a “shining” effect",
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
        Plot.plotDictionary[PLOT.TRAVEL].SubcribePlotPassByListener(this);
        return true;
    }

    public override bool Activate(string activeCase)
    {
        if (activated)
            return false;

        Debug.Log("Activate Sunnary Feather.");
        // player active animation

        Owner.RemoveItem(this);

        int playerItemSlotRemaining = Owner.itemLimit - Owner.playerItem.Count;

        int itemToAdd = Mathf.Min(playerItemSlotRemaining, SunnaryActivationCount, maxItemGrant);

        for (int i = 0; i < itemToAdd; i++)
            Owner.AddItem(Instantiate(wingsPrefab));

        Owner.ChangeMana(1);

        activated = true;

        Remove(true);
        return base.Activate(activeCase);
    }

    public void OnPlotPassBy(Player player, Plot plot)
    {
        if (player == Owner)
        {
            if (plot.Id == PLOT.TRAVEL)
            {
                Activate("");
            }
            else
            {
                Debug.Log("Subcribe to wrong plot " + plot.Id);
            }
        }
    }
    #endregion
    #region fields
    [SerializeField] private bool activated = false;
    #endregion

    private void Start()
    {
        LoadData();
    }
    public override bool Remove(bool triggerEvent)
    {
        Destroy(gameObject);
        return base.Remove(triggerEvent);
    }
}
