using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PhaseScreens
{
    PlotBuyUI,
    TempleBuyUI,
    MarketUpgradeUI,
    FreeCardUI,
    PlotRebuyUI,
    ShopUI,
    TempleRebuyUI,
}

public class StopPhaseUI : MonoBehaviour
{
    #region UI Properties
    [SerializeField]
#if UNITY_EDITOR
    [RequireInterface(typeof(UIScreen))]
#endif
    List<Component> _uiScreen;
    List<UIScreen> listUIScreen => _uiScreen.ConvertAll<UIScreen>(x => (x as UIScreen));
    #endregion

    #region Properties
    private static StopPhaseUI _ins;
    public static StopPhaseUI Ins
    {
        get 
        {
            if (_ins == null) Debug.LogWarning("Something trying to access Stop Phase UI when it's not been Init. " +
                 "Make sure you have 1 instance of this script attach to game object and it's enabled " +
                 "Make sure you check null before doing anything with the return instance");
            return _ins; 
        }
    }
    #endregion

    #region Callback

    public delegate void OnDeactivate(PhaseScreens phaseScreens);
    private event OnDeactivate onDeactiveCallback;

    #endregion

    #region Methods
    public void Activate(PhaseScreens screenType, Plot plot)
    {
        Debug.Log((int)screenType);
        foreach (var screen in listUIScreen)
        {
            Debug.Log((int)(screen.GetScreenType()));
            if (screen.GetScreenType() == screenType)
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
            if (screen.GetScreenType() == screenType)
            {
                screen.Deactivate();
                onDeactiveCallback?.Invoke(screenType);
                onDeactiveCallback = null;
                return;
            }
        }
    }

    public void SubcribeOnDeactive(PhaseScreens screenType, OnDeactivate functionCallback)
    {
        onDeactiveCallback = functionCallback;
    }
    #endregion

    #region Unity Methods
    private void Start()
    {
        _ins = this;
    }

    #endregion
}
