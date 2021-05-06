using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EventListManager : MonoBehaviourPunCallbacks
{
    public Queue<PlayerBasedAction> EventActions;

    public void Awake()
    {
        Locator.MarkInstance(this);

        EventActions = new Queue<PlayerBasedAction>();

        EventActions.Enqueue(new A1Event());
        //EventActions.Enqueue(new A2Event());
        //EventActions.Enqueue(new A3Event());
        //EventActions.Enqueue(new B1Event());
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