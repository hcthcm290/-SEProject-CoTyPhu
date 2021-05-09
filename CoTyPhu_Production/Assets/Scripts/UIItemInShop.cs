using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.UI.Button;

public class UIItemInShop : MonoBehaviour
{
    public BaseItem value;

    public void Init(BaseItem initItem)
    {
        value = initItem;
        SetInfo();
    }

    public void SetInfo()
    {
        transform.Find("ItemName").GetComponent<Text>().text = value.Name;
        transform.Find("ItemImage").GetComponent<Image>().sprite = value.gameObject.GetComponent<Image>().sprite;
        transform.Find("PanelPrice/Price").GetComponent<Text>().text = value.Price.ToString();
        transform.Find("Button").GetComponent<Button>().onClick.AddListener(Buy);
    }

    public void Buy()
    {
        Debug.Log("Da mua");
        Shop.Ins.RemoveItemFromShop(value);
        Destroy(gameObject);
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
