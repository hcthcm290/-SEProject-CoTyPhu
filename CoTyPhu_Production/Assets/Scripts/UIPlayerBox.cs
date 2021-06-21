using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using System.Linq;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIPlayerBox : MonoBehaviourPun
{
    #region UI properties
    [SerializeField] Image merchantImage;
    [SerializeField] Text playerName;
    [SerializeField] Button ultimateButton;
    [SerializeField] TextMeshProUGUI manaText;
    [SerializeField] Text goldTable;
    [SerializeField] Bank bank;
    [SerializeField] GameObject manaBarInside;
    #endregion

    // id of the shop ui, if this id is greater to the number of player in room, this ui get disable
    [SerializeField] int id;

    public static Dictionary<Player, UIPlayerBox> UILocation = new Dictionary<Player, UIPlayerBox>(); 
    public Player player;
    public Image MerchantImage
    {
        get => merchantImage;
    }

    public void Init(Player initPlayer)
    {
        player = initPlayer;
        UILocation[player] = this;
        SetInfo();
    }

    #region Activate Skill
    [PunRPC]
    private void ActivateSkillServer(int idPlayer)
    {
        if (!PhotonNetwork.IsMasterClient) return;

        photonView.RPC("ActivateSkillClient", RpcTarget.AllViaServer, idPlayer);
    }

    [PunRPC]
    private void ActivateSkillClient(int idPlayer)
    {
        Player player = TurnDirector.Ins.GetPlayer(idPlayer);

        player.GetMerchant().Skill.Activate();
    }

    public void RequestActivateSkill()
    {
        photonView.RPC("ActivateSkillServer", RpcTarget.MasterClient, player.Id);
    }

    //public void ActivateSkill()
    //{
    //    player.GetMerchant().Skill.Activate();
    //}
    #endregion

    public void SetInfo()
    {
        merchantImage.sprite = player.GetMerchant().gameObject.GetComponent<Image>().sprite;
        playerName.text = player.Name;

        if(ultimateButton != null)
        {
            ultimateButton.onClick.AddListener(RequestActivateSkill);
        }

        //transform.Find("PanelPrice/Price").GetComponent<Text>().text = value.Price.ToString();
        //transform.Find("Button").GetComponent<Button>().onClick.AddListener(Buy);

        if (transform.Find("PlayerBox/ItemBox") != null)
        {
            SetItems();

            SetMana();
            SetGold();
        }
    }

    public void SetGold()
    {

        goldTable.text = bank.MoneyPlayer(player).ToString("#,##0");
    }
    
    public void SetMana()
    {
        manaText.text = player.GetMana().ToString() + "/" + player.GetMerchant().MaxMana.ToString();
        SetActivateSkill();
    }

    public void SetActivateSkill()
    {
        if (ultimateButton != null)
        {
            ultimateButton.interactable = player.GetMerchant().Skill.CanActivate();
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
        bank.GoldChange += ListenGoldChange;
        player.ActivateChange += SetActivateSkill;
        UILocation[player] = this;
        transform.Find("FloatingNotification").GetComponent<FloatingNotification>().Owner = player;

        if(id > PhotonNetwork.PlayerList.Length)
        {
            this.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(player != null && player.GetMerchant() != null)
        {
            var scale = manaBarInside.transform.localScale;
            scale.x = (float)player.GetMana() / player.GetMerchant().MaxMana;
            manaBarInside.transform.localScale = scale;
        }
    }
}
