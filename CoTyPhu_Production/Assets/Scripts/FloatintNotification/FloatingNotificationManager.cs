using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingNotificationManager : MonoBehaviour
{
    public List<FloatingNotification> listNotiBox = new List<FloatingNotification>();
    #region Singleton
    private static FloatingNotificationManager _ins;
    public static FloatingNotificationManager Ins
    {
        get
        {
            return _ins;
        }
    }
    #endregion
    void Start()
    {
        _ins = this;
    }
    public void AddManaNotification(int mana, Player p)
    {
        foreach(FloatingNotification f in listNotiBox)
        {
            if(f.Owner == p)
            {
                f.AddManaNotification(mana);
            }
        }
    }
    public void AddGoldNotification(int gold, Player p)
    {
        foreach (FloatingNotification f in listNotiBox)
        {
            if (f.Owner == p)
            {
                f.AddGoldNotification(gold);
            }
        }
    }
}
