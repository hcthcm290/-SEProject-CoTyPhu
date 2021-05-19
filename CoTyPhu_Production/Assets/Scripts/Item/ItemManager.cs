using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class ItemManager : MonoBehaviourPun
{
    #region Singleton
    private static ItemManager _ins;
    public static ItemManager Ins
    {
        get
        {
            return _ins;
        }
    }
    #endregion

    #region Properties
    public List<BaseItem> _itemPool = new List<BaseItem>();
    public List<BaseItem> ItemPool
    {
        get { return _itemPool; }
        //set { _itemPool = value; }
    }

    /// <summary>
    /// Dictionany:
    ///     Key: shop's owner's id
    ///     Value: List of Item
    /// </summary>
    private Dictionary<int, List<BaseItem>> _listItemInShop;

    #endregion

    void Start()
    {
        _ins = this;
        _listItemInShop = new Dictionary<int, List<BaseItem>>();
        InitItemPool();
    }

    #region Item Pool
    private void InitItemPool()
    {
        //      var jsonTextFile = Resources.Load<TextAsset>("shopdata");

        //      DataItem[] temp = JsonHelper.FromJson<DataItem>(jsonTextFile.text);
        //Debug.Log(JsonHelper.FromJson<DataItem>(jsonTextFile.text));

        //      foreach (DataItem i in temp)
        //      {
        //          for (int j = 0; j < i.amount; j++)
        //          {
        //              AddItemToPool(Resources.Load<BaseItem>(i.item));
        //          }
        //      }

        AddItemToPool(Resources.Load<BaseItem>("Item001_WandererDice"));
        AddItemToPool(Resources.Load<BaseItem>("Item001_WandererDice"));
        AddItemToPool(Resources.Load<BaseItem>("Item001_WandererDice"));
        AddItemToPool(Resources.Load<BaseItem>("Item001_WandererDice"));
        AddItemToPool(Resources.Load<BaseItem>("Item003_IceDice"));
        AddItemToPool(Resources.Load<BaseItem>("Item003_IceDice"));
        AddItemToPool(Resources.Load<BaseItem>("Item003_IceDice"));
        AddItemToPool(Resources.Load<BaseItem>("Item003_IceDice"));
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
    #endregion

    #region Shop

    [PunRPC]
    private void RequestNewShopServer(int idPlayer, int itemCount)
    {
        if (!PhotonNetwork.IsMasterClient) return;

        if(!_listItemInShop.ContainsKey(idPlayer))
        {
            _listItemInShop.Add(idPlayer, new List<BaseItem>());
        }

        List<BaseItem> listItem = new List<BaseItem>(ItemPool);
        listItem.AddRange(_listItemInShop[idPlayer]);

        List<BaseItem> randomItems = new List<BaseItem>();

        while (randomItems.Count < itemCount && listItem.Count > 0)
        {
            int index = Random.Range(0, ItemPool.Count);
            BaseItem item = listItem[index];

            randomItems.Add(item);
        }

        int[] result = randomItems.ConvertAll<int>(x => x.Id).ToArray();

        photonView.RPC("RequestNewShopClient", RpcTarget.AllBufferedViaServer, idPlayer, (object)(result));
    }

    [PunRPC]
    private void RequestNewShopClient(int idPlayer, object o_itemsID)
    {
        if (!_listItemInShop.ContainsKey(idPlayer))
        {
            _listItemInShop.Add(idPlayer, new List<BaseItem>());
        }

        _itemPool.AddRange(_listItemInShop[idPlayer]);
        _listItemInShop[idPlayer].Clear();

        int[] itemsID = o_itemsID as int[];

        foreach(int id in itemsID)
        {
            BaseItem item = _itemPool.Find(x => x.Id == id);
            _itemPool.Remove(item);
            _listItemInShop[idPlayer].Add(item);
        }

        requestNewShopCallback.Complete(new List<BaseItem>(_listItemInShop[idPlayer]));
    }

    private FutureTask<List<BaseItem>> requestNewShopCallback;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="idPlayer">Shop's owner's id</param>
    /// <param name="itemCount"></param>
    /// <returns></returns>
    public Future<List<BaseItem>> RequestNewShop(int idPlayer, int itemCount)
    {
        FutureTask<List<BaseItem>> futureItems = new FutureTask<List<BaseItem>>();
        requestNewShopCallback = futureItems;

        photonView.RPC("RequestNewShopServer", RpcTarget.MasterClient, idPlayer, itemCount);

        return futureItems.GetFuture();
    }


    [PunRPC]
    public void RequestBuyItemServer(int idPlayer, int idItem)
    {
        if (!PhotonNetwork.IsMasterClient) return;

        if (!_listItemInShop.ContainsKey(idPlayer))
        {
            _listItemInShop.Add(idPlayer, new List<BaseItem>());
        }

        photonView.RPC("RequestBuyItemClient", RpcTarget.AllBufferedViaServer, idPlayer, idItem);
    }

    [PunRPC]
    public void RequestBuyItemClient(int idPlayer, int idItem)
    {
        if (!_listItemInShop.ContainsKey(idPlayer))
        {
            _listItemInShop.Add(idPlayer, new List<BaseItem>());
        }

        var item = _listItemInShop[idPlayer].Find(x => x.Id == idItem);
        _listItemInShop[idPlayer].Remove(item);

        var player = TurnDirector.Ins.GetPlayer(idPlayer);
        player.AddItem(item);

        requestBuyCallback.Complete(true);
    }


    private FutureTask<bool> requestBuyCallback;
    public Future<bool> RequestBuyItem(int idPlayer, int idItem)
    {
        FutureTask<bool> requestBuy = new FutureTask<bool>();
        requestBuyCallback = requestBuy;

        photonView.RPC("RequestBuyItemServer", RpcTarget.MasterClient, idPlayer, idItem);

        return requestBuy.GetFuture();
    }

    #endregion
}
