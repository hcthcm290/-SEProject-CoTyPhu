using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInit : MonoBehaviour
{
    public GameObject plotManager;
    public DiceManager diceManager;
    public TurnBaseManager turnBaseManager;
    public PlayerControl playerPrefab;
    // Start is called before the first frame update
    void Start()
    {
        PlayerControl p1 = Instantiate(playerPrefab, Vector3.one, Quaternion.identity);
        p1.name = "A";
        p1.diceManager = diceManager;
        p1.plotManager = plotManager;
        p1.turnBaseManager = turnBaseManager;

        turnBaseManager.listPlayer.Enqueue(p1);

        PlayerControl p2 = Instantiate(playerPrefab, Vector3.one, Quaternion.identity);
        p2.name = "B";
        p2.diceManager = diceManager;
        p2.plotManager = plotManager;
        p2.turnBaseManager = turnBaseManager;

        turnBaseManager.listPlayer.Enqueue(p2);

        PlayerControl p3 = Instantiate(playerPrefab, Vector3.one, Quaternion.identity);
        p3.name = "C";
        p3.diceManager = diceManager;
        p3.plotManager = plotManager;
        p3.turnBaseManager = turnBaseManager;

        turnBaseManager.listPlayer.Enqueue(p3);

        PlayerControl p4 = Instantiate(playerPrefab, Vector3.one, Quaternion.identity);
        p4.name = "D";
        p4.diceManager = diceManager;
        p4.plotManager = plotManager;
        p4.turnBaseManager = turnBaseManager;

        turnBaseManager.listPlayer.Enqueue(p4);

        turnBaseManager.ResetGame();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
