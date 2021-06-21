using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EventListManager : MonoBehaviourPunCallbacks
{
    public Queue<EventAction> EventActions = new Queue<EventAction>();
    public Dictionary<EventAction, Sprite> EventSprite = new Dictionary<EventAction, Sprite>();
    public List<Sprite> Sprite_In;

    public EventListManager()
    {
        Locator.MarkInstance(this);
    }

    public void Awake()
    {
        List<EventAction> Events = new List<EventAction>();

        Events.Add(new A1Event());
        Events.Add(new A2Event());
        Events.Add(new A3Event());
        Events.Add(new B1Event());
        Events.Add(new B2Event());

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
    public void ScrambleList(int seed)
    {
        System.Random rng = new System.Random(seed);
        EventActions = new Queue<EventAction>(EventActions.OrderBy(item => rng.NextDouble()).ToList());
    }

    public void Start()
    {
        if (PhotonNetwork.IsMasterClient)
            photonView.RPC("ScrambleList", RpcTarget.AllBuffered, Random.Range(int.MinValue, int.MaxValue));
    }

    public EventAction GetAction(Player target)
    {
        EventAction result = EventActions.Dequeue();
        EventActions.Enqueue(result);
        result.target = target;

        return result;
    }
}