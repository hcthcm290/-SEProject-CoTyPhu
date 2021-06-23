using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunnarySundial : SunnaryItem, IPlotPassByListener
{
    [SerializeField] int manaPerPrisoner = 1;
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

    #region Base class override
    public override bool LoadData()
    {
        return true;
    }

    public override bool StartListen()
    {
        Debug.Log("Start Listening");
        Plot.plotDictionary[PLOT.PRISON].SubcribePlotPassByListener(this);

        return true;
    }

    public override bool Remove(bool triggerEvent)
    {
        base.Remove(triggerEvent);

        return true;
    }

    public override bool Activate(string activeCase)
    {
        base.Activate(activeCase);
        if (this.gameObject != null)
        {
            Destroy(this.gameObject, 0.1f);
        }
        Plot.plotDictionary[PLOT.PRISON].UnsubcribePlotPassByListner(this);
        return true;
    }
    #endregion

    #region Unity methods
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    #endregion

    #region Handle Event

    public void OnPlotPassBy(Player player, Plot plot)
    {
        Debug.Log("Receive On Plot Pass by");
        if(player == Owner)
        {
            if(plot.Id == PLOT.PRISON)
            {
                Debug.Log("Activate Sunnary Sundial");

                PlotPrison prison = plot as PlotPrison;

                int prisonerCount = prison.AllPlayerImprisonDurations.Count;

                int totalBonusMana = manaPerPrisoner * prisonerCount;

                Owner.ChangeMana(totalBonusMana);

                /* Thanh, why didn't you delete the item when it's used?
                 */
                // Thanh: My bad bro
                Owner.RemoveItem(this);
                ItemManager.Ins.AddItemToPool(this);
                //*/

                Activate("");
                if(this.gameObject != null)
                {
                    Destroy(this.gameObject, 0.1f);
                }
            }
            else
            {
                Debug.Log("Subcribe to wrong plot " + plot.Id);
            }
        }
    }

    #endregion
}
