using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlotEvent : Plot
{
    public static GameObject EventModel;
    public static GameObject EventDeck;
    [SerializeField]
    private GameObject eventModel;
    [SerializeField]
    private GameObject eventDeck;

    public PlotEvent(PLOT id, string name, string description) : base(id, name, description)
    {

    }

    // Start is called before the first frame update
    public override void Start()
    {
        EventModel = eventModel;
        EventDeck = eventDeck;
        base.Start();
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
    }
    public override IAction ActionOnEnter(Player obj)
    {
        // Get specific event
        EventAction eventAction = EventListManager.GetInstance().GetAction(obj);

        // Pack UI actions
        ActionList result = new ActionList();

        LambdaCompletableAction action = new LambdaCompletableAction(null);                 // 3
        action.preAction = () => {
            EventDeck.SetActive(true);                                                      // 1
            // Create Event Card 
            GameObject EventCardModel = Instantiate(EventModel);
            // ... with the corresponding event sprite
            SpriteRenderer EventCardRenderer = EventCardModel.GetComponentInChildren<SpriteRenderer>();
            EventCardRenderer.sprite
                = EventListManager.GetInstance().EventSprite[eventAction];
            // When target is reached, show the skip button.
            EventCardRenderer
                .GetComponent<EventCardDraw>()
                .ListenTargetReached(
                    new LambdaAction(() =>
                    {
                        SkipButtonUI skipButtonUI = SkipButtonUI.GetInstance();
                        
                        skipButtonUI.Enable();
                        // When the skip button is clicked, destroy the EventCard
                        // and hide the event deck
                        // note: skip button UI is disabled on click
                        skipButtonUI.ListenClick(
                            new LambdaAction(() =>
                            {
                                EventDeck.SetActive(false);                                 // 1
                                Destroy(EventCardModel);
                                // Then FINALLY mark this completable action as complete
                                action.PerformOnComplete();                                 // 3
                            }));
                    }));
        };

        result.AddBlockingAction(action);
        result.AddNonBlockAction(eventAction);
        /* LOGICAL BUG
        // TODO
        result.OnActionComplete = new LambdaAction(() =>
        {
            TurnDirector.Ins.EndOfPhase();
        });
        //*/
        return result;
    }
}

public abstract class EventAction : IAction
{
    public Player target;
    public abstract void PerformAction();
}

