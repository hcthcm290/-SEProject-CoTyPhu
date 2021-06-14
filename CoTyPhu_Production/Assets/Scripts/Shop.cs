using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

/// <summary>
/// Code cac chuc nang cua giao dien cua hang
/// </summary>
public class Shop : MonoBehaviour, UIScreen
{
	public class DataItem
    {
		public string item;
		public int amount;
    }

    //Singleton
    public static Shop Ins;
	//  Events ----------------------------------------


	//  Properties ------------------------------------
	

	public List<BaseItem> ItemInShop
    {
		get { return _itemInShop; }
		//set { _itemInShop = value; }
	}

	public int MaxSellItem
	{
		get { return _maxSellItem; }
		//set { _maxSellItem = value; }
	}

	//  Fields ----------------------------------------
	public List<BaseItem> _itemInShop = new List<BaseItem>();
	private List<UIItemInShop> _UIitemInShop = new List<UIItemInShop>(); //not public
	private int _maxSellItem = 3;

	public Player playerUsingShop;
	private bool initialized = false;

    public UIItemInShop ItemInShopPrefab;

	//  Initialization --------------------------------
	public Shop()
	{

	}

    //  Methods ---------------------------------------
    public void Start()
    {
		Ins = this;
		transform.Find("ShopNoticeBoard/SkipButton/Image").GetComponent<Button>().onClick.AddListener(() => {
            StopPhaseUI.Ins.Deactive(PhaseScreens.ShopUI);
        });
		transform.Find("ShopNoticeBoard/RefreshButton/Image").GetComponent<Button>().onClick.AddListener(Reload);
		//InitPool();
		//LoadNewShop(3);
	}

    private void Update()
    {
        if(!initialized && playerUsingShop.gameObject.activeSelf == true)
        {
			initialized = true;

			ItemManager.Ins.InitShopData().then((x) => {
				LoadNewShop(3);
			});

			
		}
    }

    public void Open(Player p)
    {
		playerUsingShop = p;
		transform.GetChild(0).gameObject.SetActive(true);
    }

	public void Close()
	{
		playerUsingShop = null;
		transform.GetChild(0).gameObject.SetActive(false);
	}

	public void Reload()
    {
		LoadNewShop(MaxSellItem);
    }

	public BaseItem GetRandomItem()
	{
		return null;
	}

	public void LoadNewShop(int maxShop)
    {
		Future<List<BaseItem>> futureItems = ItemManager.Ins.RequestNewShop(playerUsingShop.Id, maxShop);

		futureItems.then((List<BaseItem> result) =>
		{
			foreach (UIItemInShop i in _UIitemInShop)
			{
                if(i != null)
                {
                    Destroy(i.gameObject);

                }
            }

			_UIitemInShop.Clear();
			ItemInShop.Clear();


			foreach (var item in result)
			{
				ItemInShop.Add(item);
			}

			foreach (BaseItem item in ItemInShop)
			{
				UIItemInShop u = Instantiate(ItemInShopPrefab, this.transform.Find("ShopNoticeBoard/Scroll View/Viewport/Content"));
				u.Init(item);
				_UIitemInShop.Add(u);
			}
		});
    }

	public bool AddItemToShop(BaseItem item)
	{
		ItemInShop.Add(item);
		return true;
	}

	public bool RemoveItemFromShop(BaseItem item)
    {
		ItemInShop.Remove(item);
		return true;
    }

	public void BuyItem(BaseItem item)
    {

    }

    public PhaseScreens GetScreenType()
    {
        return PhaseScreens.ShopUI;
    }

    public void SetPlot(Plot plot)
    {

    }

    public void Activate()
    {
        this.gameObject.SetActive(true);
    }

    public void Deactivate()
    {
        this.gameObject.SetActive(false);
    }

    //  Event Handlers --------------------------------

    #region Legacy Code
    //public List<BaseItem> ItemPool
    //{
    //	get { return _itemPool; }
    //	//set { _itemPool = value; }
    //}

    //public List<BaseItem> _itemPool = new List<BaseItem>();

    //public bool AddItemToPool(BaseItem item)
    //{
    //	ItemPool.Add(item);
    //	return true;
    //}

    //public bool RemoveItemFromPool(BaseItem item)
    //{
    //	ItemPool.Remove(item);
    //	return true;
    //}

    //public void InitPool()
    //{
    //	//      var jsonTextFile = Resources.Load<TextAsset>("shopdata");

    //	//      DataItem[] temp = JsonHelper.FromJson<DataItem>(jsonTextFile.text);
    //	//Debug.Log(JsonHelper.FromJson<DataItem>(jsonTextFile.text));

    //	//      foreach (DataItem i in temp)
    //	//      {
    //	//          for (int j = 0; j < i.amount; j++)
    //	//          {
    //	//              AddItemToPool(Resources.Load<BaseItem>(i.item));
    //	//          }
    //	//      }

    //	AddItemToPool(Resources.Load<BaseItem>("Item001_WandererDice"));
    //	AddItemToPool(Resources.Load<BaseItem>("Item001_WandererDice"));
    //	AddItemToPool(Resources.Load<BaseItem>("Item001_WandererDice"));
    //	AddItemToPool(Resources.Load<BaseItem>("Item001_WandererDice"));
    //	AddItemToPool(Resources.Load<BaseItem>("Item003_IceDice"));
    //	AddItemToPool(Resources.Load<BaseItem>("Item003_IceDice"));
    //	AddItemToPool(Resources.Load<BaseItem>("Item003_IceDice"));
    //	AddItemToPool(Resources.Load<BaseItem>("Item003_IceDice"));
    //}

    //public void LoadNewShop(int maxShop)
    //{
    //	while (ItemInShop.Count > 0)
    //	{
    //		BaseItem item = ItemInShop[0];
    //		RemoveItemFromShop(item);
    //		//AddItemToPool(item);
    //	}

    //	foreach (UIItemInShop i in _UIitemInShop)
    //	{
    //		Destroy(i.gameObject);
    //	}
    //	_UIitemInShop.Clear();

    //	while (ItemInShop.Count < maxShop && ItemPool.Count > 0)
    //	{
    //		int index = Random.Range(0, ItemPool.Count);
    //		BaseItem item = ItemPool[index];
    //		RemoveItemFromPool(item);
    //		AddItemToShop(item);
    //	}

    //	foreach (BaseItem item in ItemInShop)
    //	{
    //		UIItemInShop u = Instantiate(ItemInShopPrefab, this.transform.Find("ShopNoticeBoard/Scroll View/Viewport/Content"));
    //		u.Init(item);
    //		_UIitemInShop.Add(u);
    //	}

    //}
    #endregion
}
