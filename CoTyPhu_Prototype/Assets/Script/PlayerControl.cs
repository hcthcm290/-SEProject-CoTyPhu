using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    /// <summary>
    /// Status show if player has build a house at its turn
    /// </summary>
    protected bool Builded = false;

    public GameObject plotManager;
    public DiceManager diceManager;
    public TurnBaseManager turnBaseManager;

    public int cur_location = 0;
    protected int dest_location;

    protected Vector3 prev_position;
    protected Vector3 next_position;


    protected float jump_delay_count;
    public float jump_delay = 0.5f;

    public int numberOfDices = 2;
    public int currentNumberOfDices;

    public int state_jail = 1; //1 - not in jail, 0 - in jail
    public int number_of_moving_turn = 0;

    public int turn_maximum_count = 0;
    public int turn_maximum_limit = 3;

    private Gold gold;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = SetNewPostition(0);
        next_position = transform.position;
        jump_delay_count = jump_delay;
        currentNumberOfDices = numberOfDices;
        gold = GetComponent<Gold>();
    }

    // Update is called once per frame
    public void Update()
    {
        Debug.Log(turnBaseManager.phase);
        if(number_of_moving_turn <= 0)
        {

        }
        if (number_of_moving_turn > 0)
        {
            if (turnBaseManager.phase == 0)
            {
                if (Input.GetKeyDown(KeyCode.A))
                {
                    turnBaseManager.turn_count++;
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
                    ResetBuildStatus();
                }
                else if (!Builded && Input.GetKeyDown(KeyCode.B))
                {
                    BuildAHouse();
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
                    turnBaseManager.turnOfPlayer = p;
                    p.number_of_moving_turn++;
                    turnBaseManager.listPlayer.Enqueue(p);
                    turn_maximum_count = 0;
                }
                turnBaseManager.phase = 0;
                return;
            }
        }
    }

    protected void Jump()
    {
        Jump(1);
    }

    protected void Jump(int step)
    {
        dest_location = cur_location + step;
        if (dest_location >= 32)
        {
            dest_location -= 32;
        }
    }

    protected Vector3 SetNewPostition(int id)
    {
        Vector3 result = Vector3.zero;

        BasePlot p = plotManager.GetComponent<PlotManager>().listPlot.Find(p => p.plotID == id);
        Vector3 d = p.transform.position;
        result = new Vector3(d.x, transform.position.y, d.z);

        return result;
    }

    public bool CanBuild()
    {
        if(number_of_moving_turn > 0 && !Builded)
        {
            return true;
        }
        return false;
    }

    public void BuildAHouse()
    {
        if (!CanBuild())
            return;
        PlotManager pm = plotManager.GetComponent<PlotManager>();

        BasePlot plot = pm.listPlot.Find((x) => x.plotID == cur_location);
        if (plot is Plot_House)
        {
            Plot_House plot_house = (plot as Plot_House);
            if (plot_house.owner != this && plot_house.owner != null)
            {
                return;
            }
            else if(plot_house.owner == this)
            {
                UpgradeAHouse(plot_house);
                return;
            }

            if (gold.amount < (plot as Plot_House).cost)
            {
                Debug.LogWarning("Not enough money");
                return;
            }
            BuildingPoint bp = plot.GetComponent<BuildingPoint>();

            if (bp != null)
            {
                if (name == "A")
                {
                    bp.Build(1);
                }
                else
                {
                    bp.Build(2);
                }
                (plot as Plot_House).owner = this;
                Builded = true;
                gold.amount -= (plot as Plot_House).cost;
            }
        }
    }

    protected void UpgradeAHouse(Plot_House plot_house)
    {
        BuildingPoint bp = plot_house.GetComponent<BuildingPoint>();

        if (bp != null)
        {
            bp.Build(bp.currentHouseID + 1);
            Builded = true;
            gold.amount -= (plot_house as Plot_House).cost;
        }
    }

    /// <summary>
    /// After reset, the player have the ability to build again
    /// </summary>
    protected void ResetBuildStatus()
    {
        Builded = false;
    }
}
