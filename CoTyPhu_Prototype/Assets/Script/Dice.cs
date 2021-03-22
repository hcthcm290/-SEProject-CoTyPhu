using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Dice : MonoBehaviour
{
    public int dice_result = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //code to show result on screen then destroy
    }

    public void GetValue()
    {
        dice_result = Random.Range(1, 6);
    }
}
