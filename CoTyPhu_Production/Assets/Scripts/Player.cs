using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Player : MonoBehaviour, IDiceListener
{
    [SerializeField] List<IGoldReceiveChange> _listStatusGoldReceive = new List<IGoldReceiveChange>();
    // Properties ------------------------------------
    [SerializeField] int _id;
    public int Id
    {
        get { return _id; }
        set { _id = value; }
    }
    private bool hasLost = false;
    public bool HasLost { get => hasLost; set => hasLost = value; }
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
            Dice.SubscribeDiceListener(this);
            _notSubcribeDice = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(_notSubcribeDice)
        {
            Dice.SubscribeDiceListener(this);
            _notSubcribeDice = false;
        }
    }
    #endregion

    #region Movement
    public class ActionPlayerMove : IAction
    {
        public Player player;
        public List<int> rollResult;
        System.Action onComplete;
        public ActionPlayerMove(Player player, List<int> rollResult, System.Action onComplete)
        {
            this.player = player;
            this.rollResult = new List<int>(rollResult);
            this.onComplete = onComplete;
        }

        public void PerformAction()
        {
            player.MoveTo(rollResult.Sum(), onComplete);
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
    /// When the Player reach the target location, Will call OnActionComplete
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
    public void MoveTo(int plotsToMove, System.Action onComplete)
    {
        AddMovementToQueue(plotsToMove);


        UIActions.AddOnActionComplete(onComplete);

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
                    if(this.Location_PlotID == PLOT.PRISON)
                    {
                        StopPhaseUI.Ins.Activate(PhaseScreens.FreeCardUI, Plot.plotDictionary[Location_PlotID]);
                        StopPhaseUI.Ins.SubcribeOnDeactive(PhaseScreens.FreeCardUI, (PhaseScreens screen) =>
                        {
                            if(screen == PhaseScreens.FreeCardUI)
                            {
                                btnRoll.gameObject.SetActive(true);
                            }
                        });
                    }
                    else
                    {
                        btnRoll.gameObject.SetActive(true);
                    }


                    /*
                    if(obj.hasCard(FreeCard))
                    {
                        StopPhaseUI.Ins.Activate(PhaseScreens.FreeCardUI, this);
                    }
                    else
                    {
                        TurnDirector.Ins.EndOfPhase();
                    }
                    */
                }
                break;
            case Phase.Move:
                {
                    var plot = Plot.plotDictionary[Location_PlotID];

                    // Check if player is imprisoned, skip the phase move
                    if (plot is PlotPrison &&
                       (plot as PlotPrison).IsImprisoned(this))
                    {
                        if (MinePlayer)
                        {
                            TurnDirector.Ins.EndOfPhase();
                        }
                        else
                        {

                        }
                    }
                    else
                    {
                        UpdatePhaseMove();
                    }
                }
                break;
            case Phase.Stop:
                {
                    //Debug.Log("********PhaseStop********");
                    var plot = Plot.plotDictionary[Location_PlotID];
                    plot.ActiveOnEnter(this);
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
                    System.Action onComplete = () =>
                    {
                        _currentPhaseState = PhaseState.end;
                        UpdatePhaseMove();
                    };
                    IAction action = new ActionPlayerMove(this, diceRoll, onComplete);
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

    #endregion

    #region Temporary Area

    [SerializeField] InputField diceResultInput1;
    [SerializeField] InputField diceResultInput2;
    public void RollCheat()
    {
        List<int> result = new List<int>();
        int diceResult1;
        int.TryParse(diceResultInput1.text, out diceResult1);

        int diceResult2;
        int.TryParse(diceResultInput2.text, out diceResult2);

        result.Add(diceResult1);
        result.Add(diceResult2);

        if (TurnDirector.Ins.IsMyTurn(Id))
        {
            btnRoll.gameObject.SetActive(false);
        }

        Dice.Ins().CheatRoll(_id, result);
    }

    #endregion

    /// <summary>
    /// This function receive callback result from Dice when its finish rolling
    /// </summary>
    /// <param name="idPlayer"></param>
    /// <param name="result"></param>
    public void OnRoll(int idPlayer, List<int> result)
    {
        Debug.Log(result[0] + ":" + result[1]);

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

            TurnDirector.Ins.EndOfPhase();
        }
    }

    public int GetDiceListenerPriority()
    {
        return 2;
    }

    /// <summary>
    /// Get Player name for PlayerBox
    /// </summary>
    public string Name;

    /// <summary>
    /// Get Merchant Infomation for PlayerBox
    /// </summary>
    [SerializeField] private BaseMerchant merchant;
    
    public BaseMerchant GetMerchant()
    {
        return merchant;
    }

    public void LockMerchant()
    {
        //check to active only in merchant picking
    }

    /// <summary>
    /// Mana Process
    /// </summary>
    [SerializeField] private int _mana = 0;

    public int GetMana()
    {
        return _mana;
    }

    public void ChangeMana(int amount)
    {
        _mana += amount;
        if(_mana >= GetMerchant().MaxMana)
        {
            _mana = GetMerchant().MaxMana;
        }
    }

    public void ResetMana()
    {
        _mana = 0;
    }

    /// <summary>
    /// Get Gold For UI
    /// </summary>
    public int Gold()
    {
        //Call the bank to get this player gold here
        return 0;
    }

    /// <summary>
    /// Item list
    /// </summary>
    public List<BaseItem> playerItem;
    public int itemLimit = 3;

    public bool AddItem(BaseItem item)
    {
        Debug.Log("Adding item: " + item.Type);
        playerItem.Add(item);
        item.Owner = this;
        ItemsChange?.Invoke();
        return true;
    }

    public bool RemoveItem(BaseItem item)
    {
        playerItem.Remove(item);
        ItemsChange?.Invoke();

        return true;
    }

    public delegate void ItemChangeHandler();
    public event ItemChangeHandler ItemsChange;

    #region Method
    public void AddStatus(IGoldReceiveChange newStatus)
    {
        if (_listStatusGoldReceive == null)
        {
            _listStatusGoldReceive = new List<IGoldReceiveChange>();
        }
        if (!_listStatusGoldReceive.Contains(newStatus))
        {
            _listStatusGoldReceive.Add(newStatus);
        }
    }

    public void RemoveStatus(IGoldReceiveChange status)
    {
        _listStatusGoldReceive.Remove(status);
    }
    #endregion
}
