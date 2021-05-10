using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Player : MonoBehaviour, IDiceListener
{
    // Properties ------------------------------------
    [SerializeField] int _id;
    public int Id
    {
        get { return _id; }
        set { _id = value; }
    }
    public MoveStraightEvenly moveComponent = null;
    public PLOT Location_PlotID;
    bool _isBroke;
    bool _notSubcribeDice = true;
    [SerializeField] bool minePlayer;
    public bool MinePlayer
    {
        get { return minePlayer; }
    }
    [SerializeField] Button btnRoll;

    // Internal, saves the Actions the UI is supposed to do
    ActionList UIActions = new ActionList();

    Plot _currentPlot; 
    //ghi chú: tạo một method trong Plot là GetNextPlot() để truy cập tới Plot kế tiếp dễ dàng
    // Done! Author: Long

    enum PhaseState
    {
        start,
        ongoing,
        end,
    }

    PhaseState _currentPhaseState;

    #region Unity Methods

    // Start is called before the first frame update
    void Start()
    {
        // Long:
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
    
        // Thanh:
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
    #endregion


    #region Movement
    protected class ActionPlayerMove : IAction
    {
        public Player player;
        public List<int> rollResult;
        public ActionPlayerMove(Player player, List<int> rollResult)
        {
            this.player = player;
            this.rollResult = new List<int>(rollResult);
        }

        public void PerformAction()
        {
            player.MoveTo(rollResult.Sum());
        }
    }
    /// <summary>
    /// Commands Player to move to target Plot
    /// This will NOT move the player step-by-step.
    /// </summary>
    /// <param name="plotID"></param>
    public void MoveTo(PLOT plotID)
    {
        //Debug.Log(Plot.plotDictionary[plotID].transform.position);
        moveComponent.Target = Plot.plotDictionary[plotID].transform.position;

        Location_PlotID = plotID;
    }
    /// <summary>
    /// Return an Action that commands Player to move to target Plot
    /// This will NOT move the player step-by-step.
    /// When the Player reach the target location, Will call nextAction in the UIActions list
    /// </summary>
    /// <param name="plotID"></param>
    /// <returns></returns>
    public ICompletableAction ActionMoveTo(PLOT plotID)
    {
        LambdaCompletableAction action = new LambdaCompletableAction(new LambdaAction(() =>
        {
            // Content of the Action
            MoveTo(plotID);
        }), null);

        action.preAction = () =>
        {
            // Before Moveto, register OnComplete to moveComponent
            moveComponent.ListenTargetReached(new LambdaAction(action.PerformOnComplete));
        };

        return action;
    }
    /// <summary>
    /// Commands Player to move a number of Plot
    /// This WILL trigger PassBy Effect
    /// </summary>
    /// <param name="plotsToMove"></param>
    public void MoveTo(int plotsToMove)
    {
        AddMovementToQueue(plotsToMove);


        UIActions.AddOnActionComplete(() =>
        {
            _currentPhaseState = PhaseState.end;
            UpdatePhaseMove();
        });

        UIActions.PerformAction();
    }
    public void AddMovementToQueue(int plotsToMove)
    {
        int iter = ((int)Location_PlotID + 1) % Plot.PLOT_AMOUNT;

        for (int i = 0; i < plotsToMove; i++)
        {
            PLOT cur = (PLOT)iter;

            // Queue the action move to next tile
            UIActions.AddBlockingAction(ActionMoveTo(cur));

            // Queue the action activate OnTilePass, which is non-blocking
            IAction temp = Plot.plotDictionary[cur].ActionOnPass(this);
            if (temp != null)
                UIActions.AddNonBlockAction(temp);

            // Go to next tile.
            iter = (iter + 1) % Plot.PLOT_AMOUNT;
        }
    }
    #endregion

    #region Phases
    public void StartPhase(Phase phaseID)
    {
        _currentPhaseState = PhaseState.start;
        switch (phaseID)
        {
            case Phase.Dice:
                if (minePlayer)
                {
                    btnRoll.gameObject.SetActive(true);
                }
                break;
            case Phase.Move:
                {
                    UpdatePhaseMove();
                }
                break;
            case Phase.Stop:
                {
                    //Debug.Log("********PhaseStop********");
                    if (minePlayer)
                    {
                        var plot = Plot.plotDictionary[Location_PlotID];

                        //*
                        // Thắng, tại sao ko đặt cái này trong Plot.ActionOnEnter / ActiveOnEnter
                        if (plot is PlotConstructionMarket)
                        {
                            PlotConstructionMarket plot_mk = plot as PlotConstructionMarket;

                            if (plot_mk.Owner == null)
                            {
                                StopPhaseUI.Ins.Activate(PhaseScreens.PlotBuyUI, Plot.plotDictionary[Location_PlotID]);
                            }
                            else if (plot_mk.Owner.Id == _id)
                            {
                                // TODO
                                // Receive 1 mana

                                // Activate Market Upgrade UI
                                StopPhaseUI.Ins.Activate(PhaseScreens.MarketUpgradeUI, Plot.plotDictionary[Location_PlotID]);
                            }
                            else if (plot_mk.Owner.Id != _id)
                            {
                                // TODO
                                // Receive 2 mana

                                // Pay the rent

                                // Active Market Rebuy UI

                                TurnDirector.Ins.EndOfPhase();
                            }
                        }
                        else if (plot is PlotConstructionTemple)
                        {
                            PlotConstructionTemple plot_tmp = plot as PlotConstructionTemple;

                            if (plot_tmp.Owner == null)
                            {
                                // TODO
                                // Receive 1 mana

                                // Activate Temple Buy UI
                                StopPhaseUI.Ins.Activate(PhaseScreens.TempleBuyUI, Plot.plotDictionary[Location_PlotID]);
                            }
                            else if (plot_tmp.Owner.Id == _id)
                            {
                                // TODO
                                // Receive 2 mana

                                TurnDirector.Ins.EndOfPhase();
                            }
                            else if (plot_tmp.Owner.Id != _id)
                            {
                                // TODO
                                // Receive 1 mana

                                // Pay the temple

                                // Active Market Rebuy UI

                                TurnDirector.Ins.EndOfPhase();
                            }
                        }
                        else if(plot is PlotEvent)
                        {
                            plot.ActiveOnEnter(this);

                            // TurnDirector.Ins.EndOfPhase();
                        }
                        else
                        {
                            TurnDirector.Ins.EndOfPhase();
                        }

                        // Testing event
                        /*/
                        if (!(plot is PlotPrison))
                            Plot.plotDictionary[PLOT.EVENT1].ActiveOnEnter(this);
                        else
                            TurnDirector.Ins.EndOfPhase();
                        //*/
                    }
                }
                break;
            case Phase.Extra:
                break;
        }
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

    public void UpdatePhaseMove()
    {
        switch (_currentPhaseState)
        {
            case PhaseState.start:
                {
                    // TODO: Get dice roll
                    List<int> diceRoll = Dice.Ins().GetLastResult();

                    // Modify dice roll Post-roll
                    IAction action = new ActionPlayerMove(this, diceRoll);
                    // status.ModifyAction(action)

                    // 
                    action.PerformAction();

                    _currentPhaseState = PhaseState.ongoing;
                }
                break;
            case PhaseState.ongoing:
                break;
            case PhaseState.end:
                if (minePlayer)
                {
                    TurnDirector.Ins.EndOfPhase();
                }
                break;
        }
    }

    public void OnEndOfMove()
    {
        TurnDirector.Ins.EndOfPhase();
    }

    public void UpdatePhaseStop()
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
    
    public void Roll()
    {
        Dice.Ins().Roll(_id);

        if(TurnDirector.Ins.IsMyTurn(Id))
        {
            btnRoll.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// This function receive callback result from Dice when its finish rolling
    /// </summary>
    /// <param name="idPlayer"></param>
    /// <param name="result"></param>
    public void OnRoll(int idPlayer, List<int> result)
    {
        Debug.Log(result.ToArray());

        /// Do some fancy animation here
        /*
        if (isdouble(result))
        {
            Action AnimationAction = new LambdaAction(() =>
            {
                SubscribeWhatever(new LambdaAction(() =>
                {
                    CallNextAction();
                }));

                // Animation
            });
            UIActions.Enqueue(AnimationAction);
            AddMovementToQueue(result.Sum());
            UIActions.Enqueue(AnimationAction);
            
        
            if (UIActions.Count > 0)
                UIActions.Dequeue().PerformAction();
        }//*/

                        // only the one who roll & that is control by me can announce end of phase
                        if (idPlayer == Id && minePlayer)
        {
            Debug.Log("end of phase");
            TurnDirector.Ins.EndOfPhase();
        }
    }

    #endregion
}
