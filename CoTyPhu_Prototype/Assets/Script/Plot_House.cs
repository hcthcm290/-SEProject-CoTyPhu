using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plot_House : BasePlot
{
    [SerializeField]
    private int _cost;
    [SerializeField]
    public PlayerControl owner;

    public int cost
    {
        get { return _cost; }
    }

    // Start is called before the first frame update
    void Start()
    {
        owner = null;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
