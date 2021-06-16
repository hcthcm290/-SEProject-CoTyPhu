using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlotTax : Plot
{
    #region Constructor
    PlotTax(PLOT id, string name, string description)
        :
        base(id, name, description)
    {

    }
    #endregion

    #region base class
    public override IAction ActionOnEnter(Player player)
    {
        return new LambdaAction(() =>
        {
            // Calculate the tax fee
            int currentPlayerMoney = Bank.Ins.MoneyPlayer(player);
            int totalPlotNetworth = 0;

            foreach(var plotPair in Plot.plotDictionary)
            {
                if(plotPair.Value is PlotConstructionMarket)
                {
                    PlotConstructionMarket plot = plotPair.Value as PlotConstructionMarket;

                    if(plot.Owner == player)
                    {
                        int plotNetwork = plot.Price + (int)(0.5 * plot.UpgradeFee(0, plot.Level));

                        totalPlotNetworth += plotNetwork;
                    }
                }
            }

            int totalTax = (int)(taxFactor * (currentPlayerMoney + totalPlotNetworth));

            Bank.Ins.TakeMoney(player, totalTax, false);

            Bank.Ins.AddMoneyToLuckyDraw(100);

            if(player.MinePlayer)
            {
                TurnDirector.Ins.EndOfPhase();
            }
        });
    }
    #endregion

    #region properties
    [SerializeField] float taxFactor;
    #endregion

    #region Unity methods
    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();
    }
    #endregion
}
