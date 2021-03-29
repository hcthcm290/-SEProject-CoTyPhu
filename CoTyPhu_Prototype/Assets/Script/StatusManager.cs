using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusManager : MonoBehaviour
{
    public List<BaseStatusDetail> list_status = new List<BaseStatusDetail>();
    // Start is called before the first frame update
    void Start()
    {
        TurnBaseManager.ins.to_next_turn += ToNextTurn;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddStatusToList(BaseStatusDetail s)
    {
        s.turn_start = TurnBaseManager.ins.turn_count;
        list_status.Add(s);
        //ss.ApplyStatus();
    }

    

    public void ToNextTurn (object sender, TurnEventArgs e)
    {
  
    }
}
