using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePlot : MonoBehaviour
{
    public int plotID = 0;
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
        Debug.Log("Passing by " +  this.gameObject.name);
    }

    public void ActivePlotEffect(PlayerControl p)
    {
        Debug.Log("Stop at " + this.gameObject.name);
    }
}
