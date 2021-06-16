using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Camera master;
    public Player p1;
    public Player p2;
    public Player p3;
    public Player p4;

    private static CameraManager _ins;
    public static CameraManager Ins
    {
        get
        {
            return _ins;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        _ins = this;
    }

    public void ResetAll()
    {
        master.gameObject.SetActive(false);
        p1.transform.Find("Camera")?.gameObject.SetActive(false);
        p2.transform.Find("Camera")?.gameObject.SetActive(false);
        p3.transform.Find("Camera")?.gameObject.SetActive(false);
        p4.transform.Find("Camera")?.gameObject.SetActive(false);
    }

    public void ShowExtraCamera(Player p)
    {
        ResetAll();
        p.transform.Find("Camera")?.gameObject.SetActive(true);
    }

    public void BackToCameraMain()
    {
        ResetAll();
        master.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
