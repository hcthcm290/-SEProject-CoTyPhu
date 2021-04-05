using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNetwork : MonoBehaviour
{
    [SerializeField]
    public List<Transform> points;
    
    [SerializeField]
    int currentIndex;

    int countMove;

    [SerializeField]
    float speed;


    // Start is called before the first frame update
    void Start()
    {
        countMove = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(countMove > 0)
        {
            if(transform.position == points[currentIndex].position)
            {
                currentIndex = (currentIndex + 1) % points.Count;
                countMove--;
            }
            else
            {
                MoveTo(points[currentIndex].position);
            }

            if(countMove <= 0)
            {
                TurnBaseManagerNetwork._ins.FinishGoTo();
            }
        }
    }

    private void MoveTo(Vector3 position)
    {
        if (Vector3.Distance(position, transform.position) <= speed * Time.deltaTime)
        {
            transform.position = position;
        }
        else
        {
            transform.position += (position - transform.position).normalized * speed * Time.deltaTime;
        }
    }

    public void MoveTo(int diceResult)
    {
        countMove = diceResult;
    }
}
