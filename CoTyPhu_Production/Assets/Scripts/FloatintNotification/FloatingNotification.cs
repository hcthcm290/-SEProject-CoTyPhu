using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingNotification : MonoBehaviour
{
    public Player Owner;
    public NotificationObject template;

    Queue<NotificationObject> _notiQueue = new Queue<NotificationObject>();

    float floating_delay = 0.5f;
    float floating_delay_count = 0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            AddManaNotification(2);
        }
        if (floating_delay_count == 0)
        {
            if (_notiQueue.Count > 0)
            {
                floating_delay_count = floating_delay;
                var noti = _notiQueue.Dequeue();
                noti.Show();
            }
        }
        else
        {
            floating_delay_count -= Time.deltaTime;
            if(floating_delay_count < 0)
            {
                floating_delay_count = 0;
            }
        }
    }

    public void AddManaNotification(int mana)
    {
        var no = Instantiate(template, transform);
        no.Init(mana, NotificationType.Mana);

        _notiQueue.Enqueue(no);
    }
    public void AddGoldNotification(int gold)
    {
        var no = Instantiate(template, transform);
        no.Init(gold, NotificationType.Gold);

        _notiQueue.Enqueue(no);
    }
    public void AddItemNotification(string itemName, bool isGain)
    {
        var no = Instantiate(template, transform);
        no.Init(itemName, NotificationType.Item, isGain);

        _notiQueue.Enqueue(no);
    }
}
