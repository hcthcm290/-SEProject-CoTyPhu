using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Player : MonoBehaviour, IDiceListener, IPlotPassByListener
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
    int _rank;
    public int Rank
    {
        get => _rank;
        set => _rank = value;
    }

    int _finalNetworth;
    public int FinalNetworth
    {
        get => _finalNetworth;
        set => _finalNetworth = value;
    }

    Vector3 dest_look;

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

    public bool didDice = false;

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
        Plot.plotDictionary[PLOT.START].SubcribePlotPassByListener(this);
        Plot.plotDictionary[PLOT.PRISON].SubcribePlotPassByListener(this);
        Plot.plotDictionary[PLOT.FESTIVAL].SubcribePlotPassByListener(this);
        Plot.plotDictionary[PLOT.TRAVEL].SubcribePlotPassByListener(this);
        //Thang
        //LockMerchant(merchant);
        dest_look = transform.eulerAngles;
    }

    // Update is called once per frame
    void Update()
    {
        if(_notSubcribeDice)
        {
            Dice.SubscribeDiceListener(this);
            _notSubcribeDice = false;
        }

        float temp_y = transform.eulerAngles.y;
        float old = temp_y;
        if(dest_look.y != temp_y)
        {
            temp_y += Time.deltaTime * 180;
            if (temp_y >= 360)
            {
                temp_y = 0;
            }
            if (old < 90 && temp_y > 90)
            {
                temp_y = 90;
            }
            if (old < 180 && temp_y > 180)
            {
                temp_y = 180;
            }
            if (old < 270 && temp_y > 270)
            {
                temp_y = 270;
            }

            transform.eulerAngles = new Vector3(0, temp_y, 0);
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
                    didDice = false;
                    ActivateChange?.Invoke();
                    if (this.Location_PlotID == PLOT.PRISON)
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
            case Phase.Shop:
                {
                    if(minePlayer)
                    {
                        // Open shop
                        StopPhaseUI.Ins.Activate(PhaseScreens.ShopUI, null);

                        StopPhaseUI.Ins.SubcribeOnDeactive(PhaseScreens.ShopUI, (PhaseScreens screen) =>
                        {
                            if(screen == PhaseScreens.ShopUI)
                            {
                                TurnDirector.Ins.EndOfPhase();
                            }
                        });
                    }
                    break;
                }
            case Phase.Extra:
                break;
        }
    }

    public delegate void ActivateChangeHandler();
    public event ActivateChangeHandler ActivateChange;

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
                    CameraManager.Ins.ShowExtraCamera(this);

                    // Play animation
                    merchant.MerchantAnimator.SetBool("walking", true);

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
                {
                    CameraManager.Ins.BackToCameraMain();
                    merchant.MerchantAnimator.SetBool("walking", false);
                    if (minePlayer)
                    {
                        TurnDirector.Ins.EndOfPhase();
                    }
                    break;
                }
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
        didDice = true;
        ActivateChange?.Invoke();
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

    public void LockMerchant(BaseMerchant get_merchant)
    {
        merchant = get_merchant;
        //check to active only in merchant picking
        merchant = Instantiate(get_merchant, this.transform);
        merchant.Init();
        merchant.Skill.Owner = this;
        try
        {
            var newStatus = Instantiate(merchant.PassiveSkill, this.transform);
            newStatus.PassiveSetup(this);
        }
        catch(Exception e)
        {

        }
        MerchantLock?.Invoke();
    }

    public delegate void MerchantLockHandler();
    public event MerchantLockHandler MerchantLock;

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
        ManaChange?.Invoke();
    }

    public void ResetMana()
    {
        _mana = 0;
        ManaChange?.Invoke();
    }

    public delegate void ManaChangeHandler();
    public event ManaChangeHandler ManaChange;

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
    public int CalculateNetworth()
    {
        int currentPlayerMoney = Bank.Ins.MoneyPlayer(this);
        int totalPlotNetworth = 0;

        foreach (var plotPair in Plot.plotDictionary)
        {
            if (plotPair.Value is PlotConstructionMarket)
            {
                PlotConstructionMarket plot = plotPair.Value as PlotConstructionMarket;

                if (plot.Owner == this)
                {
                    int plotNetwork = plot.Price + (int)(0.5 * plot.UpgradeFee(0, plot.Level));

                    totalPlotNetworth += plotNetwork;
                }
            }
        }

        return currentPlayerMoney + totalPlotNetworth;
    }
    public void AddStatus(BaseStatus newStatus)
    {
        //CanGainStatus();
        if (newStatus is IGoldReceiveChange)
        {
            if (_listStatusGoldReceive == null)
            {
                _listStatusGoldReceive = new List<IGoldReceiveChange>();
            }
            if (!_listStatusGoldReceive.Contains((IGoldReceiveChange)newStatus))
            {
                _listStatusGoldReceive.Add((IGoldReceiveChange)newStatus);
            }
        }
        StatusAdding?.Invoke(newStatus);
    }

    public void RemoveStatus(BaseStatus status)
    {
        if (status is IGoldReceiveChange)
        {
            _listStatusGoldReceive.Remove((IGoldReceiveChange)status);
        }
        Destroy(status.gameObject);
    }

    public delegate void StatusAddingHandler(BaseStatus status);
    public event StatusAddingHandler StatusAdding;
    #endregion

    public void OnPlotPassBy(Player player, Plot plot)
    {
        if (player == this)
        {
            if (plot.Id == PLOT.START)
            {
                dest_look= new Vector3(0, 90, 0);
            }
            if (plot.Id == PLOT.PRISON)
            {
                dest_look = new Vector3(0, 180, 0);
            }
            if (plot.Id == PLOT.FESTIVAL)
            {
                dest_look = new Vector3(0, 270, 0);
            }
            if (plot.Id == PLOT.TRAVEL)
            {
                dest_look = new Vector3(0, 0, 0);
            }
        }
    }
}
