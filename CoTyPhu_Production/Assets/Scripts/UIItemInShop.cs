using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.UI.Button;

public class UIItemInShop : MonoBehaviour
{
    public BaseItem value;
    public Player playerBuying;

    public void Init(BaseItem initItem)
    {
        value = initItem;
        SetInfo();
    }

    public void SetInfo()
    {
        if(value == null)
        {
            Debug.Log("Null value in UIItemShop");
        }
        transform.Find("ItemName").GetComponent<Text>().text = value.Name;
        transform.Find("ItemImage").GetComponent<Image>().sprite = value.gameObject.GetComponent<Image>().sprite;
        transform.Find("PanelPrice/Price").GetComponent<Text>().text = value.Price.ToString();
        var temp = transform.Find("Button").GetComponent<Button>().onClick;
        temp.AddListener(Buy);
        temp.AddListener(() => SoundManager.Ins.Play(AudioClipEnum.Select));
        playerBuying = Shop.Ins.playerUsingShop;
    }

    public void Buy()
    {
        if (playerBuying.playerItem.Count < playerBuying.itemLimit)
        {
            //if (playerBuying.AddItem(value))
            //{
            //    Shop.Ins.RemoveItemFromShop(value);
            //    Destroy(gameObject);
            //}

            Future<bool> result = ItemManager.Ins.RequestBuyItem(playerBuying.Id, value.Id);
            result.then((bool requestResult) =>
            {
                Shop.Ins.RemoveItemFromShop(value);
                //Shop.Ins._UIItemInShop.Remove(value);
                Destroy(gameObject);
            });
        }
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
