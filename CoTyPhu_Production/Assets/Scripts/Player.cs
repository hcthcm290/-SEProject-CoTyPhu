using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    [SerializeField] int id;
    bool _isBroke;

    Plot _currentPlot; //ghi chú: tạo một method trong Plot là GetNextPlot() để truy cập tới Plot kế tiếp dễ dàng

    enum PhaseState
    {
        start,
        ongoing,
        end,
    }

    PhaseState _currentPhaseState;

    // Start is called before the first frame update
    void Start()
    {
        
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

    public void UpdatePhaseMain()
    {
        switch(_currentPhaseState)
        {
            case PhaseState.start:
                {
                    _currentPlot.ActionOnEnter(this);
                    _currentPhaseState++;
                }
                break;
            case PhaseState.ongoing:
                {

                }
                break;
            case PhaseState.end:
                {
                    
                }
                break;
            default:
                break;
        }
    }
}
