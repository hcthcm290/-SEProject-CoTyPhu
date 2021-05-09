using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//  Namespace Properties ------------------------------

//  Class Attributes ----------------------------------
public enum PLOT
{
    #region PLOTNAME DEFINITION
    START = 0,
    A1 = 1,
    MINIGAME = 2,
    A2 = 3,
    TEMPLE1 = 4,
    EVENT1 = 5,
    B1 = 6,
    B2 = 7,
    PRISON = 8,
    C1 = 9,
    C2 = 10,
    TEMPLE2 = 11,
    C3 = 12,
    EVENT2 = 13,
    D1 = 14,
    D2 = 15,
    FESTIVAL = 16,
    E1 = 17,
    E2 = 18,
    TEMPLE3 = 19,
    F1 = 20,
    EVENT3 = 21,
    F2 = 22,
    F3 = 23,
    TRAVEL = 24,
    G1 = 25,
    G2 = 26,
    TEMPLE4 = 27,
    EVENT4 = 28,
    H1 = 29,
    TAX = 30,
    H2 = 31,
    #endregion
}

/// <summary>
/// THIS CLASS HOLD ALL PROPERTIES AND METHODS THAT ALL TYPES OF PLOT NEED
/// </summary>
public class Plot : MonoBehaviour
{
    public const int PLOT_AMOUNT = 32;
    //  Events ----------------------------------------


    //  Properties ------------------------------------
    public PLOT Id { get => _id; }
    public string Name { get => _name; }
    public string Description { get => _description; }


    //  Fields ----------------------------------------
    [SerializeField]protected PLOT _id;
    [SerializeField]protected string _name;
    [SerializeField]protected string _description;

    public static Dictionary<PLOT, Plot> plotDictionary = new Dictionary<PLOT, Plot>();

    //  Initialization --------------------------------
    public Plot(PLOT id, string name, string description)
    {
        this._id = id;
        this._name = name;
        this._description = description;

    }

    public virtual void Start()
    {
        if (!plotDictionary.ContainsKey(_id))
            plotDictionary[_id] = this;
        else
            Debug.LogError("Duplicate plot id: " + this + ",\n" + plotDictionary[_id]);
    }
    public virtual void Update()
    {
        
    }
    //  Methods ---------------------------------------
    public void ActiveOnPass(dynamic obj)
    {
        // the 'this' is important for polymorphism
        this.ActionOnPass(obj).PerformAction();
    }
    public virtual IAction ActionOnPass(Player obj)
    {
        return null;
    }
    public void ActiveOnEnter(dynamic obj)
    {
        // the 'this' is important for polymorphism
        this.ActionOnEnter(obj).PerformAction();
    }
    public virtual IAction ActionOnEnter(Player obj)
    {
        return null;
    }

    public void ActiveOnLeave(dynamic obj)
    {
        // the 'this' is important for polymorphism
        this.ActionOnLeave(obj).PerformAction();
    }
    public virtual IAction ActionOnLeave(Player obj)
    {
        return null;
    }


    //  Event Handlers --------------------------------
}