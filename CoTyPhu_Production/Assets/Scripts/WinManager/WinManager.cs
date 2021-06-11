using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace WinCondition
{
    public abstract class WinCondition : MonoBehaviour
    {
        public string WinDescription;
        public string WinName;
        public abstract bool CheckWinner(Player candidate = null);
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
            WinConditionBank.GetInstance(),
            WinConditionLastStand.GetInstance(),
            WinConditionMarket.GetInstance(),
            WinConditionTemple.GetInstance()
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

        public void EndGame()
        {
            if (TurnDirector.Ins.GetMyPlayer().HasLost)
                ExitGame();

            TurnDirector ins = TurnDirector.Ins;
            Player myPlayer = ins.GetPlayer(ins.MyPlayer);
            bool hasWon = false;
            foreach (var item in winConditions)
            {
                bool won = item.CheckWinner(myPlayer);
                if (won)
                {
                    if (!hasWon)
                        item.ShowWinScreen();
                    hasWon |= won;
                }
            }

            // TODO: check hasWon for meta-progression
            if (hasWon)
                Debug.LogWarning("player has Won!");
            // TODO: Next button move to main screen.
            ExitGame();
        }

        public void ExitGame()
        {
            // TODO: Kick the player to the main screen.
            SceneManager.CreateScene("WinScreen");
            SceneManager.LoadScene("WinScreen");
        }

        static public IAction GetWinCheckAction()
        {
            return new WinCheckAction();
        }
    }
    public class WinCheckAction : IAction
    {
        public virtual void PerformAction()
        {
            if (Locator<WinManager>.Instance.CheckWinner())
                Locator<WinManager>.Instance.EndGame();
        }
    }
    public class WinConCheckAction : IAction
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