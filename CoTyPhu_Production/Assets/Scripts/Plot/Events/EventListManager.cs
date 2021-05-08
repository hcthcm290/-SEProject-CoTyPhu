using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EventListManager : MonoBehaviourPunCallbacks
{
    public Queue<PlayerBasedAction> EventActions = new Queue<PlayerBasedAction>();
    public Dictionary<PlayerBasedAction, Sprite> EventSprite = new Dictionary<PlayerBasedAction, Sprite>();
    public List<Sprite> Sprite_In;

    public EventListManager()
    {
        Locator.MarkInstance(this);
    }

    public void Awake()
    {
        List<PlayerBasedAction> Events = new List<PlayerBasedAction>();

        Events.Add(new A1Event());
        //Events.Enqueue(new A2Event());
        //Events.Enqueue(new A3Event());
        //Events.Enqueue(new B1Event());

        for (int i = 0; i < Events.Count; i++)
        {
            EventActions.Enqueue(Events[i]);
            EventSprite[Events[i]] = Sprite_In[i];
        }
    }

    public static EventListManager GetInstance()
    {
        return Singleton<EventListManager>.GetInstance();
    }

    [PunRPC]
    public void ScrambleList()
    {
        EventActions = new Queue<PlayerBasedAction>(EventActions.OrderBy(item => UnityEngine.Random.Range(0, 500)).ToList());
    }

    public void Start()
    {
        if (PhotonNetwork.IsMasterClient)
            photonView.RPC("ScrambleList", RpcTarget.AllBuffered);
    }

    public PlayerBasedAction GetAction(Player target)
    {
        PlayerBasedAction result = EventActions.Dequeue();
        EventActions.Enqueue(result);
        result.target = target;

        return result;
    }
}