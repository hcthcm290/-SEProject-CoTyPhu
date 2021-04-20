using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Code cac chuc nang cua giao dien cua hang
/// </summary>
public class Shop
{
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
	private List<BaseItem> _itemPool;
	private List<BaseItem> _itemInShop;
	private int _maxSellItem;

	//  Initialization --------------------------------
	public Shop()
	{

	}

	//  Methods ---------------------------------------
	public BaseItem GetRandomItem()
	{
		return null;
	}

	public bool AddItemToPool(BaseItem item)
	{
		return true;
	}

	public bool RemoveItemFromPool(int index)
    {
		return true;
    }

	public bool AddItemToShop(BaseItem item)
	{
		return true;
	}

	public bool RemoveItemFromShop(int index)
    {
		return true;
    }

	//  Event Handlers --------------------------------

}
