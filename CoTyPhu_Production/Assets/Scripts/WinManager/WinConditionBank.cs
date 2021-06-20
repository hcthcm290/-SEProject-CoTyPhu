using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace WinCondition
{
    public class WinConditionBank : WinCondition
    {
        // The Win Splash screen to be shown when this victory is achieved.
        public WinBoardUI WinScreen;
        public GameObject DrawScreen;
        public bool IsDraw = false;
        // Get Singleton Instance
        static public WinConditionBank GetInstance()
        {
            return Singleton<WinConditionBank>.GetInstance();
        }
        public WinConditionBank() {
            WinDescription = "Ngân hàng quốc tế phá sản, kinh tế đi xuống trầm trọng.\n" +
                "Là người giàu có nhất, bạn cho các chính phủ vay tiền.\n" +
                "Và tất nhiên, bạn nghiễm nhiên thâu tóm toàn bộ thế giới trong tay.\n" +
                "Ở thế giới này, phép thuật thì mạnh đấy, nhưng tiền vẫn mạnh hơn cả !";
            WinName = "Nhiều tiền có quyền chiến thắng";
            Locator.MarkInstance(this);
        }
        public override bool CheckWinner(Player candidate = null)
        {
            if (candidate != null)
            {
                int MaxMoney = Bank.Ins.AllMoneyPlayers.Values.Max();
                return Bank.Ins.MoneyBank <= 0 && Bank.Ins.AllMoneyPlayers[candidate] >= MaxMoney;
            }
            return Bank.Ins.MoneyBank <= 0;
        }

        public override void ShowWinScreen()
        {
            WinScreen.SetWinDescription(WinName, WinDescription);
            WinScreen.gameObject.SetActive(true);
        }
        // Get an Action corresponding to Checking this WinCondition
        static IAction GetWinConCheckAction()
        {
            return new WinConCheckAction(GetInstance());
        }
    }
}
