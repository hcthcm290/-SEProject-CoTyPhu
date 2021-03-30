using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiceManager : MonoBehaviour
{
    public Dice dicePrefab;
    public List<Dice> dicelist = new List<Dice>();
    public int dice_sum = 0;
    public Text DiceUI1;
    public Text DiceUI2;

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

        DiceUI1.text = dicelist[0].dice_result.ToString();
        DiceUI2.text = dicelist[1].dice_result.ToString();

        //dice_sum = 1;
    }

    public bool IsDouble()
    {
        return (dicelist[0].dice_result == dicelist[1].dice_result);
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
