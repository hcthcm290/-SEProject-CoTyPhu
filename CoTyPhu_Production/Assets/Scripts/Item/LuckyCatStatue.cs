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
        base.Remove(triggerEvent);
        if(this.gameObject != null)
        {
            Destroy(this.gameObject, 0.1f);
        }
        return true;
    }
    #endregion

    #region Properties
    [SerializeField] StatusLuckyCatStatue status;
    #endregion
}
