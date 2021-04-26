using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WinCondition
{
    public abstract class WinCondition : MonoBehaviour
    {
        public string WinDescription;
        public string WinName;
        public abstract bool CheckWinner();
        public string GetDescription()
        {
            return WinDescription;
        }
        public string GetName()
        {
            return WinName;
        }
        public abstract void ShowWinScreen();
    }

    public class WinManager : Singleton<WinManager>
    {
        WinCondition[] winConditions =
        {
    };
        void CheckSingleton()
        {
#if DEBUG
            if (this != GetInstance())
                Debug.LogError("Detect multiple instances of WinManager.");
#endif
        }

        public bool CheckWinner()
        {
            CheckSingleton();
            foreach (var item in winConditions)
            {
                if (item.CheckWinner())
                    return true;
            }
            return false;
        }

        static public Action GetWinCheckAction()
        {
            return new WinCheckAction();
        }
    }
    public class WinCheckAction : Action
    {
        public virtual void PerformAction()
        {
            Locator<WinManager>.Instance.CheckWinner();
        }
    }
    public class WinConCheckAction : Action
    {
        WinCondition winCon;
        public WinConCheckAction(WinCondition wincon)
        {
            winCon = wincon;
        }
        public virtual void PerformAction()
        {
            winCon.CheckWinner();
        }
    }
}