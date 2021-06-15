using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;

public class TurnNotifierUI : MonoBehaviour, ITurnListener
{
    bool _init = false;
    [SerializeField] TextMeshProUGUI _content;


    void Update()
    {
        if(!_init)
        {
            TurnDirector.Ins.SubscribeTurnListener(this);
            _init = true;
        }
    }

    #region Turn listener
    public void OnBeginTurn(int idPlayer)
    {
        if(TurnDirector.Ins.IsMyTurn())
        {
            _content.text = "Your turn!";
        }
        else
        {
            var player = TurnDirector.Ins.GetPlayer(idPlayer);
            _content.text = player.Name + " turn!";
        }
        
        _content.transform.localScale = Vector3.zero;
        _content.DOFade(1, 0.3f);
        _content.transform.DOScale(1, 0.5f);
        _content.DOFade(1, 1f).onComplete = () =>
        {
            _content.transform.DOScale(0, 0.3f);
            _content.DOFade(0, 0.3f);
        };
    }

    public void OnEndTurn(int idPlayer)
    {

    }
    #endregion
}
