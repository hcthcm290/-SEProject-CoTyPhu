using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FloatingArrow : MonoBehaviour
{
    public void Start()
    {
        (transform as RectTransform).DOScaleY(1.2f, 0.65f).SetLoops(-1, LoopType.Yoyo);
        (transform as RectTransform).DOAnchorPosY(280, 0.65f).SetLoops(-1, LoopType.Yoyo);
    }

    private void OnEnable()
    {
        this.gameObject.SetActive(true);
    }

    private void OnDisable()
    {
        this.gameObject.SetActive(false);
    }
}
