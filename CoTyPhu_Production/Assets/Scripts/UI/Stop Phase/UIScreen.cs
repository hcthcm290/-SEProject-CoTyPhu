using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public interface UIScreen
{
    PhaseScreens GetType();
    void SetPlot(Plot plot);

    void Activate();
    void Deactivate();
}
