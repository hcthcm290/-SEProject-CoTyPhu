using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public partial class Player { };
namespace WinCondition
{
    public class WinConditionLastStand : IWinCondition
    {
        public Player[] player =
        { 
        };
        public GameObject WinScreen;

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
        static Action GetWinConCheckAction()
        {
            return new WinConCheckAction(GetInstance());
        }
    }
}
