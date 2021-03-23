using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plot_Start : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ActivePlotPassByEffect(PlayerControl p)
    {
        p.SendMessage("GainGold", 200);
        Debug.Log("Gained 200 Gold");
    }
}
