using UnityEngine;


/// <summary>
/// PLOT.TRAVEL (24) MOVE PLAYER IMMEDIATELY TO A SELECTED PLOT 
/// </summary>
public class PlotTravel: Plot
{
	//  Events ----------------------------------------


	//  Properties ------------------------------------


	//  Fields ----------------------------------------


	//  Initialization --------------------------------
	public PlotTravel(PLOT id, string name, string description) : base(id, name, description) { }


    //  Methods ---------------------------------------
    private void PerformTravel(Plot destination)
    {
        //TODO: Move immediately to destination
    }

    public override Action ActionOnEnter(Player obj)
    {
        //TODO: Choose a plot as destination

        Plot destination = null;
        return new LambdaAction(() => {
            this.PerformTravel(destination);
        });
    }


    //  Event Handlers --------------------------------
}