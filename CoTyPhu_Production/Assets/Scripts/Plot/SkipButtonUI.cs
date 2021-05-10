using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Photon.Pun;
public class SkipButtonUI : MonoBehaviourPunCallbacks
{
    public float Countdown = 0f;
    public float MaxTime = 5f;
    public static SkipButtonUI GetInstance()
    {
        return Singleton<SkipButtonUI>.GetInstance();
    }

    public SkipButtonUI()
    {
        Locator.MarkInstance(this);
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        Countdown -= Time.deltaTime;
        if (Countdown <= 0)
        {
            PerformOnClickEvent();
        }
    }

    public void Enable()
    {
        gameObject.SetActive(true);
        Countdown = MaxTime;
    }

    private void Disable()
    {
        gameObject.SetActive(false);
    }

    List<IAction> OnClick = new List<IAction>();
    public void ListenClick(IAction action)
    {
        OnClick.Add(action);
    }
    public void PerformOnClick()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
            return;

        if (TurnDirector.Ins.IsMyTurn())
            photonView.RPC("PerformOnClickEvent", RpcTarget.All);
        else
            PerformOnClickEvent();
    } 
    [PunRPC]
    public void PerformOnClickEvent()
    {
        List<IAction> temp = OnClick;
        OnClick = new List<IAction>();

        foreach (IAction item in temp)
            item.PerformAction();

        Disable();
    }
}
