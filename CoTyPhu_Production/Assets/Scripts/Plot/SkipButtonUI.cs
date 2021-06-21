using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Photon.Pun;
using UnityEngine.UI;

public class SkipButtonUI : MonoBehaviourPunCallbacks
{
    public float Countdown = 0f;
    public float MaxTime = 5f;
    public Text text;
    public GameObject button;
    public string textContent = "Skip";
    public static SkipButtonUI GetInstance()
    {
        return Locator.GetInstance<SkipButtonUI>();
    }

    public SkipButtonUI()
    {
        Locator.MarkInstance(this);
    }

    // Start is called before the first frame update
    void Start()
    {
        if (text == null)
            text = GetComponentInChildren<Text>();
        if (button == null)
            button = GetComponentInChildren<Button>().gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        Countdown -= Time.deltaTime;
        text.text = textContent + " " + Mathf.FloorToInt(Countdown);
        if (Countdown <= 0)
        {
            if (PhotonNetwork.IsMasterClient)
                photonView.RPC("PerformOnClickEvent", RpcTarget.All);
        }
    }

    private void BaseEnable()
    {
        gameObject.SetActive(true);
        Countdown = MaxTime;

        SoundManager.Ins.PlayLooped(AudioClipEnum.Clock);
    }

    public void Enable()
    {
        BaseEnable();

        if (!button.activeSelf)
            button.SetActive(true);
    }

    public void EnableTimerOnly()
    {
        BaseEnable();

        if (button.activeSelf)
            button.SetActive(false);
    }

    public void Disable()
    {
        gameObject.SetActive(false);

        SoundManager.Ins.StopPlayLooped(AudioClipEnum.Clock);

        OnClick.Clear();
    }

    #region Event
    List<IAction> OnClick = new List<IAction>();
    /// <summary>
    /// Warning: On click action list will be cleared after event taken.
    /// </summary>
    /// <param name="action"></param>
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
        foreach (IAction item in OnClick)
            item.PerformAction();

        Disable();
    }
    #endregion
}
