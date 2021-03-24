using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    bool Builded = false;

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
    public int state_jail = 1; //1 - not in jail, 0 - in jail

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
            if(currentNumberOfDices == 2 && diceManager.IsDouble())
            {
                //code to gain a turn and move counter here

                //
                state_jail = 1;
            }
            Jump(state_jail * diceManager.dice_sum);
            state_moving = true;
            Builded = false;
        }

        if (state_moving)
        {

            if (jump_delay_count < jump_delay)
            {
                // character move to next plot
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

                // if character not reach the end_plot, get the next plot
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
        else
        {
            if (!Builded && Input.GetKeyDown(KeyCode.B))
            {
                PlotManager pm = plotManager.GetComponent<PlotManager>();

                BasePlot plot = pm.listPlot.Find((x) => x.plotID == cur_location);
                if (plot is Plot_House)
                {
                    BuildingPoint bp = plot.GetComponent<BuildingPoint>();

                    if(bp != null)
                    {
                        bp.Build(1);
                        Builded = true;
                    }
                }

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
