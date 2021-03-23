using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gold : MonoBehaviour
{
    public int amount = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GainGold(int num)
    {
        amount += num;
    }

    public void PayGold(int num)
    {
        amount -= num;
    }
}
