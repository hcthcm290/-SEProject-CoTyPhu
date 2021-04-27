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
        public Player[] player =
        { 
        };
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
        }
        public override bool CheckWinner()
        {
            throw new NotImplementedException();
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
