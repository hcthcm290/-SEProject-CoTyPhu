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

    public void ActivateSkill()
    {
        player.GetMerchant().Skill.Activate();
    }

    public void SetInfo()
    {
        if (player != null)
        {
            gameObject.SetActive(true);
            transform.Find("PlayerBox/MerchantImage").GetComponent<Image>().sprite = player.GetMerchant().gameObject.GetComponent<Image>().sprite;
            transform.Find("PlayerBox/NamePanel/Text").GetComponent<Text>().text = player.Name;

            transform.Find("Ultimate")?.GetComponent<Button>().onClick.AddListener(ActivateSkill);
            
            SetItems();

            SetMana();
            SetGold();
        }
    }

    public void SetGold()
    {
        transform.Find("PlayerBox/GoldTable/Text").GetComponent<Text>().text = Bank.Ins.MoneyPlayer(player).ToString("#,##0");
    }
    
    public void SetMana()
    {
        transform.Find("PlayerBox/ManaBar/Text").GetComponent<Text>().text = player.GetMana().ToString() + "/" + player.GetMerchant().MaxMana.ToString();

        var ultimate = transform.Find("Ultimate");
        if (ultimate != null)
        {
            ultimate.GetComponent<Button>().interactable = player.GetMerchant().Skill.CanActivate();
        }
    }

    public void SetItems()
    {

        if (transform.Find("PlayerBox/ItemBox") != null)
        {
            Queue<string> itemComponent = new Queue<string>();
            for (int i = 1; i <= player.itemLimit; i++)
            {
                itemComponent.Enqueue("Slot" + i);
            }

            for (int i = 1; i <= player.itemLimit; i++)
            {
                Transform b = transform.Find("PlayerBox/ItemBox/" + itemComponent.Dequeue());
                b.GetComponent<UIItemInPlayer>().SetNull();
                itemComponent.Enqueue("Slot" + i);
            }

            foreach (BaseItem item in player.playerItem)
            {
                Transform b = transform.Find("PlayerBox/ItemBox/" + itemComponent.Dequeue());
                b.GetComponent<UIItemInPlayer>().Init(item);
            }
        }
    }

    void ListenGoldChange(Player p)
    {
        if(p == player)
        {
            SetGold();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        player.MerchantLock += SetInfo;
        player.ItemsChange += SetItems;
        player.ManaChange += SetMana;
        Bank.Ins.GoldChange += ListenGoldChange;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
