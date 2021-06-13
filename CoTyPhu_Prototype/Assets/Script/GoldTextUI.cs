using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoldTextUI : MonoBehaviour
{
    Text text;
    public Gold gold;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        text.text = ToString();
    }

    override public string ToString()
    {
        return "Gold: " + gold.amount.ToString();
    }
}
