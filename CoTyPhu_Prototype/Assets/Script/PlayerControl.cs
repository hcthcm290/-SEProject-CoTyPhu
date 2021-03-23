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

    public int numberOfDices = 2;
    public int currentNumberOfDices;

    public bool state_moving = false;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = SetNewPostition(cur_location);
        next_position = transform.position;
        jump_delay_count = jump_delay;
        currentNumberOfDices = numberOfDices;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            diceManager.Roll(currentNumberOfDices);
            Jump(diceManager.dice_sum);
            state_moving = true;
        }

        if (state_moving)
        {

            if (jump_delay_count < jump_delay)
            {
                jump_delay_count += Time.deltaTime;
                transform.position += (next_position - prev_position) * Time.deltaTime / jump_delay;
            }
            else
            {
                if (transform.position != next_position)
                {
                    transform.position = next_position;
                    plotManager.GetComponent<PlotManager>().listPlot.Find(p => p.plotID == cur_location).ActivePlotPassByEffect(this);
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

            //code when step on the plot
            if (transform.position == SetNewPostition(dest_location))
            {
                plotManager.GetComponent<PlotManager>().listPlot.Find(p => p.plotID == dest_location).ActivePlotEffect(this);
                state_moving = false;
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

        BasePlot p = plotManager.GetComponent<PlotManager>().listPlot.Find(p => p.plotID == id);
        Vector3 d = p.transform.position;
        result = new Vector3(d.x, transform.position.y, d.z);

        return result;
    }
}
