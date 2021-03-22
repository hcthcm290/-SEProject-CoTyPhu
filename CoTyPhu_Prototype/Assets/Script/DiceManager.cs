using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceManager : MonoBehaviour
{
    public Dice dicePrefab;
    public List<Dice> dicelist = new List<Dice>();
    public int dice_sum = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Roll(int numberOfDice)
    {
        ClearAllDice();

        dice_sum = 0;

        for (int i = 1; i <= numberOfDice; i++)
        {
            Dice d = Instantiate(dicePrefab, this.transform);
            d.GetValue();
            dice_sum += d.dice_result;
            dicelist.Add(d);
        }
    }

    public void ClearAllDice()
    {
        foreach (Dice dd in dicelist)
        {
            Destroy(dd.gameObject);
        }
        dicelist.Clear();
    }
}
