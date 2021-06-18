using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public interface IPayPlotFeeListener
{
    void OnPayPlotFee(Player player, PlotConstruction plot);
    PLOT? AssignedPlot { get; set; }
}
