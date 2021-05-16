using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayerBox : MonoBehaviour
{
    public Player player;

    public void Init(Player initPlayer)
    {
        player = initPlayer;
        SetInfo();
    }

    public void SetInfo()
    {
        //transform.Find("PlayerBox/ManaBar").GetComponent<Text>().text = value.Mana;
        //transform.Find("PlayerBox/MerchantImage").GetComponent<Image>().sprite = value.gameObject.GetComponent<Image>().sprite;
        //transform.Find("PanelPrice/Price").GetComponent<Text>().text = value.Price.ToString();
        //transform.Find("Button").GetComponent<Button>().onClick.AddListener(Buy);

        Queue<string> itemComponent = new Queue<string>();
        for (int i = 1; i <= player.itemLimit; i++)
        {
            itemComponent.Enqueue("Slot" + i);
        }

        foreach (BaseItem item in player.playerItem)
        {
            Transform b = transform.Find("PlayerBox/ItemBox/" + itemComponent.Dequeue());
            b.GetComponent<UIItemInPlayer>().Init(item);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        player.AddedItemToPlayer += SetInfo;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
