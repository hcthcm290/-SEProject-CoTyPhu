using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIItemInPlayer : MonoBehaviour
{
    public BaseItem value;

    public void Init(BaseItem initItem)
    {
        if (value == null)
        {
            value = initItem;
            SetInfo();
        }
    }

    public void SetInfo()
    {
        transform.GetComponent<Image>().sprite = value.gameObject.GetComponent<Image>().sprite;
        transform.GetComponent<Button>().onClick.AddListener(Detail);
    }

    public void Detail()
    {
        Debug.Log("Show Info");
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
