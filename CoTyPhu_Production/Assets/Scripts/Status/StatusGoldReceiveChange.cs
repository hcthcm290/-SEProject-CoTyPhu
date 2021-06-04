using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StatusGoldReceiveChange : BaseStatus, IGoldReceiveChange
{
    public float goldReceiveChange { get; set; }
    public Player targetPlayer;

    public float GetGoldReceiveChange(float basePrice, float delta)
    {
        delta += goldReceiveChange * basePrice;

        return delta;
    }

    public override bool LoadData()
    {
        return true;
    }

    public override bool StartListen()
    {
        if(targetPlayer != null)
        {
            targetPlayer.AddStatus(this);
            gameObject.AddComponent<ExpiredOnTurn>();
            gameObject.GetComponent<ExpiredOnTurn>().Init(this, 1);
            return true;
        }
        else
        {
            Debug.Log("Must set target player before listening");
            return false;
        }

    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public override bool Remove(bool triggerEvent)
    {
        targetPlayer.RemoveStatus(this);
        base.Remove(triggerEvent);
        Destroy(this.gameObject);

        return true;
    }

    public override bool ExcuteAction()
    {
        return true;
    }
}
