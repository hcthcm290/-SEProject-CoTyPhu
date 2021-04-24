using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public MoveStraightEvenly moveComponent = null;
    public PLOT Location_PlotID;
    [SerializeField] int id;
    bool _isBroke;

    // Internal, saves the Actions the UI is supposed to do
    Queue<Action> UIActions;
    public Action OnUIActionComplete;
    private void CallNextAction()
    {
        if (UIActions.Count > 0)
            UIActions.Dequeue().PerformAction();
        else
            OnUIActionComplete?.PerformAction();
    }

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
        //moveComponent.lockY = true;
        //moveComponent.lockX = false;
        //moveComponent.lockZ = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// Commands Player to move to target Plot
    /// This will NOT move the player step-by-step.
    /// </summary>
    /// <param name="plotID"></param>
    public void MoveTo(PLOT plotID)
    {
        moveComponent.Target = Plot.plotDictionary[plotID].transform.position;
    }
    /// <summary>
    /// Return an Action that commands Player to move to target Plot
    /// This will NOT move the player step-by-step.
    /// When the Player reach the target location, Will call nextAction in the UIActions list
    /// </summary>
    /// <param name="plotID"></param>
    /// <returns></returns>
    public Action ActionMoveTo(PLOT plotID)
    {
        Action result = new LambdaAction(() =>
        {
            // When it is done moving, Call the next action
            moveComponent.ListenTargetReached(new LambdaAction(() =>
            {
                CallNextAction();
            }));

            // Content of the Action
            MoveTo(plotID);
        });
        return result;
    }
    /// <summary>
    /// Commands Player to move a number of Plot
    /// This WILL trigger PassBy Effect
    /// </summary>
    /// <param name="plotsToMove"></param>
    public void Move(int plotsToMove)
    {
        int iter = ((int)Location_PlotID + 1) % Plot.PLOT_AMOUNT;

        for (int i = 0; i < plotsToMove; i++)
        {
            PLOT cur = (PLOT)iter;

            // Queue the action move to next tile
            UIActions.Enqueue(ActionMoveTo(cur));

            // Queue the action activate OnTilePass
            Action temp = Plot.plotDictionary[cur].ActionOnPass(this);
            // If there is an action, call the action, then ...
            if (temp != null)
                // call the next action immediately after.
                UIActions.Enqueue(new LambdaAction(temp, CallNextAction));
            
            // Go to next tile.
            iter = (iter + 1) % Plot.PLOT_AMOUNT;
        }
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
