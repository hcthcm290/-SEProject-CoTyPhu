using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunnariCrown : BaseItem, IPlotPassByListener
{
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

    #region New
    public List<BaseItem> ItemSunnari;
    #endregion

    #region Base class override
    public override bool LoadData()
    {
        return true;
    }

    public override bool StartListen()
    {
        Debug.Log("Start Listening");
        Plot.plotDictionary[PLOT.FESTIVAL].SubcribePlotPassByListener(this);

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
        if (player == Owner)
        {
            if (plot.Id == PLOT.FESTIVAL)
            {
                Debug.Log("Activate Sunnari Crown");

                Owner.ChangeMana(1);

                Future<bool> check = ItemManager.Ins.GetRandomSunnariItem(Owner.Id);
                check.then((condition) =>
                {
                    Owner.RemoveItem(this);
                });
            }
            else
            {
                Debug.Log("Subcribe to wrong plot " + plot.Id);
            }
        }
    }

    #endregion
}
