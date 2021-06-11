using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace WinCondition
{
    public class WinConditionLastStand : WinCondition
    {
        // The Win Splash screen to be shown when this victory is achieved.
        public GameObject WinScreen;

        // Get Singleton Instance
        static public WinConditionLastStand GetInstance()
        {
            return Singleton<WinConditionLastStand>.GetInstance();
        }
        public WinConditionLastStand()
        {
            WinDescription = "Là người chơi duy nhất chưa phá sản";
            WinName = "Kẻ sống sót cuối cùng";
            Locator.MarkInstance(this);
        }
        public override bool CheckWinner(Player candidate = null)
        {
            TurnDirector ins = TurnDirector.Ins;
            if (candidate == null)
                return ins.ListPlayer.Count(item => !item.HasLost) <= 1;
            else
                return ins.ListPlayer.Count(item => !item.HasLost) == 1 && !candidate.HasLost;
        }

        public override void ShowWinScreen()
        {
            WinScreen?.SetActive(true);
        }
        // Get an Action corresponding to Checking this WinCondition
        static IAction GetWinConCheckAction()
        {
            return new WinConCheckAction(GetInstance());
        }
    }
}
