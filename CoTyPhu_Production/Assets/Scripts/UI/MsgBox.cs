using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MsgBox : MonoBehaviour
{
    private static MsgBox _ins;
    public static MsgBox Ins
    {
        get { return _ins; }
    }

    [SerializeField] Text txtMsg;

    // Start is called before the first frame update
    void Start()
    {
        _ins = this;
    }

    public void Show(string content)
    {
        txtMsg.text = content;
        gameObject.SetActive(true);
    }

    public void btnOkClick()
    {
        gameObject.SetActive(false);
    }
}
