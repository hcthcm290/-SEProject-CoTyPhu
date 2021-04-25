using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IDiceListener
{
    [SerializeField] int _id;
    public int Id
    {
        get { return _id; }
        set { _id = value; }
    }
    bool _isBroke;
    bool _notSubcribeDice = true;

    // Start is called before the first frame update
    void Start()
    {
        if (Dice.Ins() != null)
        {
            Dice.Ins().SubscribeDiceListener(this);
            _notSubcribeDice = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(_notSubcribeDice)
        {
            Dice.Ins().SubscribeDiceListener(this);
            _notSubcribeDice = false;
        }
    }

    public void MoveTo(int plotID)
    {

    }

    public void StartPhase(int phaseID)
    {

    }

    private void StartPhaseDice()
    {

    }

    public void EndPhase()
    {
        
    }

    public void PausePhase()
    {

    }

    public void Roll()
    {
        Dice.Ins().Roll(_id);
    }


    public void OnRoll(int idPlayer, List<int> result)
    {
        Debug.Log(result);
    }


}
