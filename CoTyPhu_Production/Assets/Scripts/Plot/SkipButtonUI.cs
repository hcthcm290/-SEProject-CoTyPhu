using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SkipButtonUI : MonoBehaviour
{
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
        
    }

    public void Enable()
    {
        gameObject.SetActive(true);
    }

    public void Disable()
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

        foreach (IAction item in OnClick)
            item.PerformAction();
    }
}
