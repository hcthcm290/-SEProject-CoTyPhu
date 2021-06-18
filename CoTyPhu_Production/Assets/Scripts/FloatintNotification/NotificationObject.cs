using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum NotificationType
{
    Gold,
    Mana

}

public class NotificationObject : MonoBehaviour
{
    NotificationType type; //0: gold, 1: mana
    public string text;
    string color;

    float appear_time = 2f;
    float appear_time_count = 2f;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (appear_time_count == 0)
        {
            Remove();
        }
        else
        {
            transform.position += new Vector3(0, Time.deltaTime * 50, 0);
            appear_time_count -= Time.deltaTime;
            if (appear_time_count < 0)
            {
                appear_time_count = 0;
            }
        }
    }

    public void Remove()
    {
        Destroy(gameObject);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Init(int amount, NotificationType type)
    {
        if (type == NotificationType.Mana)
        {
            if (amount <= 0)
            {
                text = amount.ToString() + " Mana";
                transform.Find("Text").GetComponent<Text>().color = Color.white;
                transform.Find("Text").GetComponent<Text>().text = text;
            }
            else
            {
                text = "+ " + amount.ToString() + " Mana";
                transform.Find("Text").GetComponent<Text>().color = Color.blue;
                transform.Find("Text").GetComponent<Text>().text = text;
            }
        }

        if (type == NotificationType.Gold)
        {
            if (amount <= 0)
            {
                text = amount.ToString() + " Gold";
                transform.Find("Text").GetComponent<Text>().color = Color.red;
                transform.Find("Text").GetComponent<Text>().text = text;
            }
            else
            {
                text = "+ " + amount.ToString() + " Gold";
                transform.Find("Text").GetComponent<Text>().color = Color.yellow;
                transform.Find("Text").GetComponent<Text>().text = text;
            }
        }
    }
}
