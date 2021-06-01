using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LuckyCatStatue : BaseItem
{
    #region Base class
    public override Player Owner
    {
        get
        {
            return base.Owner;
        }
        set
        {
            base.Owner = value;
            status.Owner = value;
            StartListen();
        }
    }
    public override bool LoadData()
    {
        return true;
    }

    public override bool StartListen()
    {
        status.StartListen();
        return true;
    }

    public override bool Remove(bool triggerEvent)
    {
        Destroy(this.gameObject);
        base.Remove(triggerEvent);
        return true;
    }
    #endregion

    #region Properties
    [SerializeField] StatusLuckyCatStatue status;
    #endregion
}
