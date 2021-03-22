using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public GameObject plotManager;
    public DiceManager diceManager;

    public int cur_location = 0;
    private int dest_location;

    private Vector3 prev_position;
    private Vector3 next_position;


    private float jump_delay_count;
    public float jump_delay = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        transform.position = SetNewPostition(cur_location);
        next_position = transform.position;
        jump_delay_count = jump_delay;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            diceManager.Roll(2);
            Jump(diceManager.dice_sum);
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Jump(1);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Jump(2);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Jump(3);
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            Jump(4);
        }

        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            Jump(5);
        }

        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            Jump(6);
        }

        if (jump_delay_count < jump_delay)
        {
            jump_delay_count += Time.deltaTime;
            transform.position += (next_position - prev_position) * Time.deltaTime / jump_delay;
        }
        else
        {
            if(transform.position != next_position)
            {
                transform.position = next_position;
            }
            if (dest_location != cur_location)
            {
                prev_position = SetNewPostition(cur_location);
                cur_location += 1;
                if (cur_location >= 32)
                {
                    cur_location -= 32;
                }
                jump_delay_count = 0;
                next_position = SetNewPostition(cur_location);
            }
        }
    }

    void Jump()
    {
        Jump(1);
    }

    void Jump(int step)
    {
        dest_location = cur_location + step;
        if (dest_location >= 32)
        {
            dest_location -= 32;
        }
    }

    Vector3 SetNewPostition(int id)
    {
        Vector3 result = Vector3.zero;

        Vector3 d = plotManager.GetComponent<PlotManager>().listPlot.Find(p => p.plotID == id).transform.position;
        result = new Vector3(d.x, transform.position.y, d.z);

        return result;
    }
}
