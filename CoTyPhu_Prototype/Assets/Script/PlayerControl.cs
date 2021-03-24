using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public GameObject plotManager;
    public DiceManager diceManager;
    public TurnBaseManager turnBaseManager;

    public int cur_location = 0;
    private int dest_location;

    private Vector3 prev_position;
    private Vector3 next_position;


    private float jump_delay_count;
    public float jump_delay = 0.5f;

    public int numberOfDices = 2;
    public int currentNumberOfDices;

    public int state_jail = 1; //1 - not in jail, 0 - in jail
    public int number_of_moving_turn = 0;

    public int turn_maximum_count = 0;
    public int turn_maximum_limit = 3;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = SetNewPostition(0);
        next_position = transform.position;
        jump_delay_count = jump_delay;
        currentNumberOfDices = numberOfDices;
    }

    // Update is called once per frame
    void Update()
    {
        if(number_of_moving_turn <= 0)
        {

        }
        if (number_of_moving_turn > 0)
        {
            if (turnBaseManager.phase == 0)
            {
                if (Input.GetKeyDown(KeyCode.A))
                {
                    turn_maximum_count++;
                    diceManager.Roll(currentNumberOfDices);
                    if (currentNumberOfDices == 2 && diceManager.IsDouble())
                    {
                        if (turn_maximum_count >= turn_maximum_limit)
                        {
                            number_of_moving_turn = 1;
                            //code to fly to prison here

                            //
                            turnBaseManager.phase = 4;
                            return;
                        }
                        else
                        {
                            number_of_moving_turn++;
                        }
                        state_jail = 1;
                    }
                    Jump(state_jail * diceManager.dice_sum);
                    turnBaseManager.phase = 1;
                }
                return;
            }

            if (turnBaseManager.phase == 1)
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
                    turnBaseManager.phase = 2;
                }
                return;
            }

            if(turnBaseManager.phase == 2)
            {
                plotManager.GetComponent<PlotManager>().listPlot.Find(p => p.plotID == dest_location).ActivePlotEffect(this);
                turnBaseManager.phase = 4;
                return;
            }

            if(turnBaseManager.phase == 4)
            {
                number_of_moving_turn--;
                if (number_of_moving_turn > 0)
                {
                    
                }
                else
                {
                    PlayerControl p = turnBaseManager.listPlayer.Dequeue();
                    p.number_of_moving_turn++;
                    turnBaseManager.listPlayer.Enqueue(p);
                    turn_maximum_count = 0;
                }
                turnBaseManager.phase = 0;
                return;
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
