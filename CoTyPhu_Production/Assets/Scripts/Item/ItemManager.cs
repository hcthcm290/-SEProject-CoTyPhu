using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Linq;
using System;

public class Pair<X, Y>
{
    public X Item1;
    public Y Item2;

    public Pair(X item1, Y item2)
    {
        Item1 = item1;
        Item2 = item2;
    }
}

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
    public Dictionary<int, Pair<BaseItem, int>> _itemPool = new Dictionary<int, Pair<BaseItem, int>>();
    public Dictionary<int, Pair<BaseItem, int>> ItemPool
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
        futureTasks = new List<FutureTask<List<BaseItem>>>();
        InitItemPool();
    }

    private void Update()
    {
        
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

        AddItemToPool(Resources.Load<BaseItem>("Item001_WandererDice"), 3);
        AddItemToPool(Resources.Load<BaseItem>("Item003_IceDice"), 3);
        AddItemToPool(Resources.Load<BaseItem>("Item_Sunnary_Feather"), 8);
        AddItemToPool(Resources.Load<BaseItem>("Item_Sunnary_Sundial"), 8);
        AddItemToPool(Resources.Load<BaseItem>("Item_Three_Spades"), 8);
        AddItemToPool(Resources.Load<BaseItem>("Lucky Cat Statue"), 8);
        AddItemToPool(Resources.Load<BaseItem>("Item_Burning_Dice"), 8);
        AddItemToPool(Resources.Load<BaseItem>("Item_Mirror"), 8);
        AddItemToPool(Resources.Load<BaseItem>("Item_Sunnari_Crown"), 8);
        AddItemToPool(Resources.Load<BaseItem>("Item_Moses_Staff"), 8);
    }

    public bool AddItemToPool(BaseItem item)
    {
        if(_itemPool.ContainsKey(item.Id))
        {
            _itemPool[item.Id].Item2++;
        }
        else
        {
            _itemPool.Add(item.Id, new Pair<BaseItem, int>(item, 1));
        }
        return true;
    }

    public bool AddItemToPool(List<BaseItem> items)
    {
        foreach(var item in items)
        {
            AddItemToPool(item);
        }

        return true;
    }

    public bool AddItemToPool(BaseItem item, int number)
    {
        if (_itemPool.ContainsKey(item.Id))
        {
            _itemPool[item.Id].Item2 += number;
        }
        else
        {
            _itemPool.Add(item.Id, new Pair<BaseItem, int>(item, number));
        }
        return true;
    }

    public bool RemoveItemFromPool(BaseItem item)
    {
        if (_itemPool.ContainsKey(item.Id))
        {
            if (_itemPool[item.Id].Item2 == 0)
            {
                return false;
            }
            else
            {
                _itemPool[item.Id].Item2--;
                return true;
            }
        }
        else
        {
            return false;
        }
    }

    public BaseItem RemoveItemFromPool(int idItem)
    {
        if (_itemPool.ContainsKey(idItem))
        {
            if (GetPoolItemCount(idItem) == 0)
            {
                return null;
            }
            else
            {
                _itemPool[idItem].Item2--;
                return _itemPool[idItem].Item1;
            }
        }
        else
        {
            return null;
        }
    }

    private int GetPoolItemCount(int idItem)
    {
        if (!_itemPool.ContainsKey(idItem)) return -1;
        return _itemPool[idItem].Item2;
    }

    private BaseItem GetItemFromPool(int idItem)
    {
        return _itemPool[idItem].Item1;
    }
    #endregion

    #region Shop

    #region Methods
    /// <summary>
    /// Must not called combine with AddItemToPool
    /// </summary>
    /// <param name="idPlayer"></param>
    private void ClearShop(int idPlayer)
    {
        AddItemToPool(_listItemInShop[idPlayer]);

        _listItemInShop[idPlayer].Clear();
    }

    private void MoveItemPoolToShop(int idPlayer, int idItem)
    {
        if(GetPoolItemCount(idItem) > 0)
        {
            var poolItem = RemoveItemFromPool(idItem);

            _listItemInShop[idPlayer].Add(poolItem);
        }
        else
        {
            Debug.Log("There is not enough item in pool to move to shop");
        }
    }
    #endregion

    #region Get List Item
    [PunRPC]
    private void RequestItemServer(int idItem, int itemCount)
    {
        if (!PhotonNetwork.IsMasterClient) return;

        List<int> Source = new List<int>();

        var item = ItemPool.Where(key => key.Key == idItem);
        if (item.Count() == 0 || item.Count() > 1)
        {

        }
        else
        {
            for (int i = 0; i < item.First().Value.Item2; i++)
            {
                Source.Add(item.First().Key);
            }
        }

        List<int> ItemList = new List<int>();

        while ((ItemList.Count() < itemCount && itemCount != 0) || itemCount == 0)
        {
            if (Source.Count == 0) break;

            ItemList.Add(Source[0]);
            Source.RemoveAt(0);
        }

        int[] result = ItemList.ToArray();
        //Debug.Log("result "+ result[0].ToString() + ", " + result[1].ToString() + ", " + result[2].ToString());

        photonView.RPC("RequestItemClient", RpcTarget.MasterClient, idItem, (object)(result));
    }

    [PunRPC]
    private void RequestItemClient(int idItem, object o_itemsID)
    {
        int[] itemsID = o_itemsID as int[];

        string debug = "";
        foreach (int id in itemsID)
        {
            debug += id.ToString() + ", ";
        }

        Debug.Log("Client" + debug);

        List<BaseItem> result = new List<BaseItem>();

        foreach (int id in itemsID)
        {
            if (GetPoolItemCount(id) > 0)
            {
                var poolItem = RemoveItemFromPool(id);

                result.Add(poolItem);
            }
            else
            {
                Debug.Log("There is not enough item in pool to move to shop");
            }
        }

        requestItemCallback?.Complete(new List<BaseItem>(result));
    }

    private FutureTask<List<BaseItem>> requestItemCallback;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="idItem">id Item to get</param>
    /// <param name="itemCount">Number of item get, 0 to get all item left</param>
    /// <returns></returns>
    public Future<List<BaseItem>> RequestItem(int idItem, int itemCount)
    {
        FutureTask<List<BaseItem>> futureItems = new FutureTask<List<BaseItem>>();
        requestItemCallback = futureItems;

        photonView.RPC("RequestItemServer", RpcTarget.MasterClient, idItem, itemCount);

        return futureItems.GetFuture();
    }
    #endregion

    #region Renew Shop
    [PunRPC]
    private void RequestNewShopServer(int idPlayer, int itemCount, int numberMoney)
    {
        if (!PhotonNetwork.IsMasterClient) return;

        if (!_listItemInShop.ContainsKey(idPlayer))
        {
            _listItemInShop.Add(idPlayer, new List<BaseItem>());
        }

        ClearShop(idPlayer);

        List<int> randomItems = new List<int>();
        List<int> IDsRandomSource = new List<int>();

        foreach (var item in ItemPool)
        {
            for (int i = 0; i < item.Value.Item2; i++)
            {
                IDsRandomSource.Add(item.Key);
            }
        }

        string debug = "";
        foreach (int id in IDsRandomSource)
        {
            debug += id.ToString() + ", ";
        }

        Debug.Log("source " + debug);

        while (randomItems.Count < itemCount)
        {
            if (IDsRandomSource.Count == 0) break;

            int idItemindex = UnityEngine.Random.Range(0, IDsRandomSource.Count);
            randomItems.Add(IDsRandomSource[idItemindex]);

            IDsRandomSource.RemoveAt(idItemindex);
        }

        int[] result = randomItems.ToArray();
        Debug.Log("result " + result[0].ToString() + ", " + result[1].ToString() + ", " + result[2].ToString());

        photonView.RPC("RequestNewShopClient", RpcTarget.AllBufferedViaServer, idPlayer, (object)(result), numberMoney);
    }

    [PunRPC]
    private void RequestNewShopClient(int idPlayer, object o_itemsID, int numberMoney)
    {
        Player player = TurnDirector.Ins.GetPlayer(idPlayer);
        if(numberMoney != 0)
        {
            Bank.Ins.TakeMoney(player, numberMoney);
        }

        if (!_listItemInShop.ContainsKey(idPlayer))
        {
            _listItemInShop.Add(idPlayer, new List<BaseItem>());
        }

        ClearShop(idPlayer);

        int[] itemsID = o_itemsID as int[];

        string debug = "";
        foreach (int id in itemsID)
        {
            debug += id.ToString() + ", ";
        }


        foreach (int id in itemsID)
        {
            MoveItemPoolToShop(idPlayer, id);
        }

        requestNewShopCallback?.Complete(new List<BaseItem>(_listItemInShop[idPlayer]));
    }

    private FutureTask<List<BaseItem>> requestNewShopCallback;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="idPlayer">Shop's owner's id</param>
    /// <param name="itemCount"></param>
    /// <returns></returns>
    public Future<List<BaseItem>> RequestNewShop(int idPlayer, int itemCount, int numberMoney)
    {
        FutureTask<List<BaseItem>> futureItems = new FutureTask<List<BaseItem>>();
        requestNewShopCallback = futureItems;

        photonView.RPC("RequestNewShopServer", RpcTarget.MasterClient, idPlayer, itemCount, numberMoney);

        return futureItems.GetFuture();
    }
    #endregion

    #region Fetch shop data methods
    [PunRPC]
    private void FetchShopItemsServer(int idPlayer, string idClientRequest, int taskIndex)
    {
        if (!PhotonNetwork.IsMasterClient) return;

        if(!_listItemInShop.ContainsKey(idPlayer))
        {
            _listItemInShop.Add(idPlayer, new List<BaseItem>());
        }

        var shopItemsID = _listItemInShop[idPlayer].ConvertAll(x => x.Id).ToArray();

        var clientRequest = PhotonNetwork.PlayerList.Single(x => x.UserId == idClientRequest);

        photonView.RPC("FetchShopItemsClient", clientRequest, idPlayer, (object)(shopItemsID), taskIndex);
    }

    [PunRPC]
    private void FetchShopItemsClient(int idPlayer, object o_itemsID, int taskIndex)
    {
        var shopItemsID = o_itemsID as int[];

        if (!_listItemInShop.ContainsKey(idPlayer))
        {
            _listItemInShop.Add(idPlayer, new List<BaseItem>());
        }

        ClearShop(idPlayer);

        foreach (int itemID in shopItemsID)
        {
            MoveItemPoolToShop(idPlayer, itemID);
        }

        futureTasks[taskIndex].Complete(_listItemInShop[idPlayer]);
    }

    List<FutureTask<List<BaseItem>>> futureTasks;
    private Future<List<BaseItem>> FetchShopItems(int idPlayer)
    {
        FutureTask<List<BaseItem>> fetchTask = new FutureTask<List<BaseItem>>();
        futureTasks.Add(fetchTask);

        photonView.RPC("FetchShopItemsServer", RpcTarget.MasterClient, idPlayer, PhotonNetwork.LocalPlayer.UserId, futureTasks.Count - 1);

        return fetchTask.GetFuture();
    }

    FutureTask<bool> initShopTask;
    public Future<bool> InitShopData()
    {
        FutureTask<bool> initTask = new FutureTask<bool>();
        initShopTask = initTask;

        int totalPlayer = TurnDirector.Ins.ListPlayer.Count;
        int countPlayer = 0;

        foreach (var player in TurnDirector.Ins.ListPlayer)
        {
            var future = FetchShopItems(player.Id);
            future.then((x) =>
            {
                countPlayer++;

                if(countPlayer == totalPlayer)
                {
                    initTask.Complete(true);
                }
            });
        }

        return initTask.GetFuture();
    }

    #endregion

    #region Buy item
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
        Player player = TurnDirector.Ins.GetPlayer(idPlayer);

        if (_listItemInShop.ContainsKey(idPlayer))
        {
            var itemInShop = _listItemInShop[idPlayer].Find(x => x.Id == idItem);

            var item = Instantiate(itemInShop);

            _listItemInShop[idPlayer].Remove(item);
            Bank.Ins.TakeMoney(player, item.Price);
            player.AddItem(item);

            requestBuyCallback?.Complete(true);

            return;
        }
        else
        {
            FetchShopItems(idPlayer).then((x) =>
            {
                var itemInShop = _listItemInShop[idPlayer].Find(x => x.Id == idItem);

                var item = Instantiate(itemInShop);

                _listItemInShop[idPlayer].Remove(item);
                Bank.Ins.TakeMoney(player, item.Price);
                player.AddItem(item);

                requestBuyCallback?.Complete(true);

                return;
            });
        }
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

    #region Use item
    [PunRPC]
    private void UseItemServer(int idPlayer, int itemID)
    {
        if (!PhotonNetwork.IsMasterClient) return;

        photonView.RPC("UseItemClient", RpcTarget.AllViaServer, idPlayer, itemID);
    }

    [PunRPC]
    private void UseItemClient(int idPlayer, int itemID)
    {
        Player player = TurnDirector.Ins.GetPlayer(idPlayer);
        BaseItem item = player.playerItem.Find(x => x.Id == itemID);

        if(item == null)
        {
            Debug.LogError("player dont have item to active");
            return;
        }

        if (!player.MinePlayer)
            item.AnimationEffect();
        item.Activate("");
    }

    public void RequestUseItem(int idPlayer, int itemID)
    {
        photonView.RPC("UseItemServer", RpcTarget.MasterClient, idPlayer, itemID);
    }
    #endregion

    #endregion

    #region Get A Random Sunnari Item
    [PunRPC]
    private void GetRandomtSunnariItemServer(int owner)
    {
        if (!PhotonNetwork.IsMasterClient) return;

        List<BaseItem> com = new List<BaseItem>();

        Future<List<BaseItem>> b1 = RequestItem(5, 0);
        b1.then((list) =>
        {
            Debug.Log("5");
            foreach (var item in list)
            {
                com.Add(item);
            }
            Future<List<BaseItem>> b2 = RequestItem(6, 0);
            b2.then((list) =>
            {
                Debug.Log("6");
                foreach (var item in list)
                {
                    com.Add(item);
                }
                Future<List<BaseItem>> b3 = RequestItem(7, 0);
                b3.then((list) =>
                {
                    Debug.Log("7");
                    foreach (var item in list)
                    {
                        com.Add(item);
                    }
                    Future<List<BaseItem>> b4 = RequestItem(8, 0);
                    b4.then((list) =>
                    {
                        Debug.Log("8");
                        foreach (var item in list)
                        {
                            com.Add(item);
                        }

                        int randint = UnityEngine.Random.Range(0, com.Count);

                        //
                        string debug = "";
                        foreach(BaseItem bi in com)
                        {
                            debug += bi.Id + ",";
                        }
                        Debug.LogWarning(debug);
                        //

                        BaseItem result = com[randint];
                        AddItemToPool(com);

                        int result_id = result.Id;

                        Debug.Log(result_id);

                        photonView.RPC("GetRandomSunnariItemClient", RpcTarget.AllBufferedViaServer, owner, result_id);
                    });
                });
            });
        });
        //Debug.Log("result "+ result[0].ToString() + ", " + result[1].ToString() + ", " + result[2].ToString());
    }

    [PunRPC]
    private void GetRandomSunnariItemClient(int idPlayer, int gainitem)
    {
        Player player = TurnDirector.Ins.GetPlayer(idPlayer);
        var poolitem = RemoveItemFromPool(gainitem);
        player.AddItem(poolitem);
        getRandomSunnariItemCallback?.Complete(true);
    }

    private FutureTask<bool> getRandomSunnariItemCallback;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="idItem">id Item to get</param>
    /// <param name="itemCount">Number of item get, 0 to get all item left</param>
    /// <returns></returns>
    public Future<bool> GetRandomSunnariItem(int idPlayer)
    {
        FutureTask<bool> futureItems = new FutureTask<bool>();
        getRandomSunnariItemCallback = futureItems;

        photonView.RPC("GetRandomtSunnariItemServer", RpcTarget.MasterClient, idPlayer);

        return futureItems.GetFuture();
    }
    #endregion
}
