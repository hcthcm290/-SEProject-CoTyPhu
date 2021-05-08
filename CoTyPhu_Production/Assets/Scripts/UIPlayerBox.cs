using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPlayerBox : MonoBehaviour
{
    public Player value;

    public void Init(Player initItem)
    {
        value = initItem;
        SetInfo();
    }

    public void SetInfo()
    {
        //transform.Find("PlayerBox/ManaBar").GetComponent<Text>().text = value.Mana;
        //transform.Find("PlayerBox/MerchantImage").GetComponent<Image>().sprite = value.gameObject.GetComponent<Image>().sprite;
        //transform.Find("PanelPrice/Price").GetComponent<Text>().text = value.Price.ToString();
        //transform.Find("Button").GetComponent<Button>().onClick.AddListener(Buy);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
