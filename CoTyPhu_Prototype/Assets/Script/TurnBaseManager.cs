using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnBaseManager : MonoBehaviour
{
    public PlayerControl playerPrefab;
    public int turn_count = 0;
    public int phase = 0;
    public float phase_time_limit = 15; //chua dung trong prototype
    public float phase_time_limit_count = 0;
    public Queue<PlayerControl> listPlayer = new Queue<PlayerControl>();

    // Start is called before the first frame update
    void Start()
    {
        ResetGame();
    }

    void ResetGame()
    {
        PlayerControl p = listPlayer.Dequeue();
        p.number_of_moving_turn++;
        listPlayer.Enqueue(p);
    }

    // Update is called once per frame
    void Update()
    {
        switch(phase)
        {
            case 0:
                {
                    //dice roll and active skill phase

                    //show button dice
                    //show item icon clickable
                    //show the player all plot that can go to
                    break;
                }
            case 1:
                {
                    //moving character and active passing by effect
                    break;
                }
            case 2:
                {
                    //stop character and active landing effect
                    break;
                }
            case 3:
                {
                    //delay turn for effect (this is phase for plot to activate)
                    
                    break;
                }
            case 4:
                {
                    //this phase waiting for before turn end effect
                    break;
                }
            default:
                break;

        }
    }
}
