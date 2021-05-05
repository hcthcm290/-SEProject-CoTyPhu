using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Code cac chuc nang cua giao dien cua hang
/// </summary>
public class Shop : MonoBehaviour
{
    public class TempShop
    {
        public List<TempItem> shop;
    }

	public class TempItem
    {
		public string item;
		public int amount;
    }

    //Singleton
    public static Shop Ins;
	//  Events ----------------------------------------


	//  Properties ------------------------------------
	public List<BaseItem> ItemPool
	{
		get { return _itemPool; }
		//set { _itemPool = value; }
	}

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
	public List<BaseItem> _itemPool = new List<BaseItem>();
	public List<BaseItem> _itemInShop = new List<BaseItem>();
	private List<UIItemInShop> _UIitemInShop = new List<UIItemInShop>(); //not public
	private int _maxSellItem = 3;

	public UIItemInShop ItemInShopPrefab;

	//  Initialization --------------------------------
	public Shop()
	{

	}

    //  Methods ---------------------------------------
    public void Start()
    {
		Ins = this;
		transform.Find("ShopNoticeBoard/SkipButton/Image").GetComponent<Button>().onClick.AddListener(Close);
		transform.Find("ShopNoticeBoard/RefreshButton/Image").GetComponent<Button>().onClick.AddListener(Reload);
		InitPool();
		LoadNewShop(3);
	}

	public void InitPool()
    {
        var jsonTextFile = Resources.Load<TextAsset>("shopdata");

        TempShop temp = JsonUtility.FromJson<TempShop>(jsonTextFile.text);

        Debug.Log(temp.shop[0]);

        foreach (TempItem i in temp.shop)
        {
            for (int j = 0; j < i.amount; j++)
            {
                AddItemToPool(Resources.Load<BaseItem>(i.item));
            }
        }

        //AddItemToPool(Resources.Load<BaseItem>("Item001_WandererDice"));
        //AddItemToPool(Resources.Load<BaseItem>("Item001_WandererDice"));
        //AddItemToPool(Resources.Load<BaseItem>("Item001_WandererDice"));
        //AddItemToPool(Resources.Load<BaseItem>("Item001_WandererDice"));
        //AddItemToPool(Resources.Load<BaseItem>("Item003_IceDice"));
        //AddItemToPool(Resources.Load<BaseItem>("Item003_IceDice"));
        //AddItemToPool(Resources.Load<BaseItem>("Item003_IceDice"));
        //AddItemToPool(Resources.Load<BaseItem>("Item003_IceDice"));
    }

    public void Open()
    {
		transform.GetChild(0).gameObject.SetActive(true);
    }

	public void Close()
	{
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
		while (ItemInShop.Count > 0)
        {
			BaseItem item = ItemInShop[0];
			RemoveItemFromShop(item);
			AddItemToPool(item);
        }

		foreach(UIItemInShop i in _UIitemInShop)
        {
			Destroy(i.gameObject);
        }
		_UIitemInShop.Clear();

		while (ItemInShop.Count < maxShop && ItemPool.Count > 0)
        {
			int index = Random.Range(0, ItemPool.Count);
			BaseItem item = ItemPool[index];
			RemoveItemFromPool(item);
			AddItemToShop(item);
        }

		foreach(BaseItem item in ItemInShop)
        {
			UIItemInShop u = Instantiate(ItemInShopPrefab, this.transform.Find("ShopNoticeBoard/Scroll View/Viewport/Content"));
			u.Init(item);
			_UIitemInShop.Add(u);
        }
		
    }

	public bool AddItemToPool(BaseItem item)
	{
		ItemPool.Add(item);
		return true;
	}

	public bool RemoveItemFromPool(BaseItem item)
    {
		ItemPool.Remove(item);
		return true;
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

	//  Event Handlers --------------------------------

}
