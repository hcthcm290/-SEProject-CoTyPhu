using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnBaseManager : MonoBehaviour
{
    public static TurnBaseManager ins;
    public PlayerControl playerPrefab;
    private int _turn_count = 0;

    public int turn_count
    {
        get { return _turn_count; }
        set { _turn_count = value; TurnChange(); }
    }

    public int phase = 0;
    public float phase_time_limit = 15; //chua dung trong prototype
    public float phase_time_limit_count = 0;
    public Queue<PlayerControl> listPlayer = new Queue<PlayerControl>();

    public PlayerControl turnOfPlayer;

    public delegate void TurnEventHandler(object sender, TurnEventArgs e);
    public event TurnEventHandler to_next_turn;

    // Start is called before the first frame update
    void Start()
    {
        ins = this;
    }

    public void ResetGame()
    {
        PlayerControl p = listPlayer.Dequeue();
        turnOfPlayer = p;
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

    void TurnChange()
    {
        to_next_turn?.Invoke(this, new TurnEventArgs(turn_count, turnOfPlayer));
    }
}

public class TurnEventArgs : EventArgs
{
    public TurnEventArgs(int turn_count, PlayerControl p)
    {
        this._turnCount = turn_count;
        this._turnOfPlayer = p;
    }
    // Lưu dữ liệu gửi đi từ publisher
    private int _turnCount;

    public int turnCount
    {
        get { return _turnCount; }
    }

    private PlayerControl _turnOfPlayer;

    public PlayerControl turnOfPlayer
    {
        get { return _turnOfPlayer; }
    }
}
