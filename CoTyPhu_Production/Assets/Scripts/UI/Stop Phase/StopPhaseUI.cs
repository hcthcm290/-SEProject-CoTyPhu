using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PhaseScreens
{
    PlotBuyUI,
    TempleBuyUI,
}

public class StopPhaseUI : MonoBehaviour
{
    #region UI Properties
    [SerializeField]
    [RequireInterface(typeof(UIScreen))]
    List<Component> _uiScreen;
    List<UIScreen> listUIScreen => _uiScreen.ConvertAll<UIScreen>(x => (x as UIScreen));
    #endregion

    #region Properties
    private static StopPhaseUI _ins;
    public static StopPhaseUI Ins
    {
        get { return _ins; }
    }
    #endregion

    #region Methods
    public void Activate(PhaseScreens screenType, Plot plot)
    {
        foreach (var screen in listUIScreen)
        {
            if (screen.GetType() == screenType)
            {
                screen.SetPlot(plot);
                screen.Activate();
                return;
            }
        }
    }

    public void Deactive(PhaseScreens screenType)
    {
        foreach (var screen in listUIScreen)
        {
            if (screen.GetType() == screenType)
            {
                screen.Deactivate();
                return;
            }
        }
    }
    #endregion

    #region Unity Methods
    private void Start()
    {
        _ins = this;
    }

    #endregion
}
