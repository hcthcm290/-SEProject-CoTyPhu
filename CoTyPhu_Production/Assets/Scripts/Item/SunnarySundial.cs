using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunnarySundial : BaseItem, IPlotPassByListener
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

    public override bool Activate(string activeCase)
    {
        base.Activate(activeCase);
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

                int ownerCurrentMana = Owner.GetMana();

                Owner.ChangeMana(ownerCurrentMana + totalBonusMana);

            }
            else
            {
                Debug.Log("Subcribe to wrong plot " + plot.Id);
            }
        }
    }

    #endregion
}
