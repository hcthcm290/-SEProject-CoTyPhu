using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseStatus : MonoBehaviour
{
    public TurnBaseManager turnBaseManager;
    //status duration
    public int duration_by_turn = 1;
    public int turn_start;
    //status list
    public List<BaseStatusDetail> listStatus = new List<BaseStatusDetail>();

    public virtual void InitStatus()
    {
        turn_start = turnBaseManager.turn_count;
    }    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RemoveByDurationTurn()
    {
        foreach(BaseStatusDetail b in listStatus)
        {
            b.RemoveStatus();
        }
        Destroy(this.gameObject);
    }
}
