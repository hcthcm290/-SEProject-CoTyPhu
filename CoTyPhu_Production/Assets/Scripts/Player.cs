using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public MoveStraightEvenly moveComponent;
    [SerializeField] int id;
    bool _isBroke;

    // Start is called before the first frame update
    void Start()
    {
        if (moveComponent == null)
            moveComponent = GetComponent<MoveStraightEvenly>();
        if (moveComponent == null)
        {
            gameObject.AddComponent<MoveStraightEvenly>();
            moveComponent = GetComponent<MoveStraightEvenly>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MoveTo(int plotID)
    {

    }

    public void StartPhase(int phaseID)
    {

    }

    public void EndPhase()
    {

    }

    public void PausePhase()
    {

    }
}
