using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace WinCondition
{
    public class WinConditionTemple : WinCondition
    {
        // The Win Splash screen to be shown when this victory is achieved.
        public WinBoardUI WinScreen;

        // Get Singleton Instance
        static public WinConditionTemple GetInstance()
        {
            return Singleton<WinConditionTemple>.GetInstance();
        }
        public WinConditionTemple()
        {
            WinDescription = 
                "Sau khi sử dụng đền thờ để hồi sinh Thần ABCXYZ, bạn trở thành chủ nhân của hắn.\n" +
                "Cả thế giới phải quy phục trước bạn, hoặc bạn sẽ gọi thần và hủy diệt tất cả.\n" +
                "Bạn vơ vét tất cả, sống sung sướng giữa những túi tiền, châu báu.\n" +
                "Thời kỳ của bạn đã bắt đầu, tươi sáng hay đen tối cũng đã rõ.";
            WinName = "Cánh cổng từ địa ngục";
            Locator.MarkInstance(this);
        }
        public override bool CheckWinner(Player candidate = null)
        {
            foreach (PLOT plot in Plot.TemplePlot)
            {
                PlotConstructionTemple temple = Plot.plotDictionary[plot] as PlotConstructionTemple;

                if (temple.Owner == null)
                    return false;

                if (candidate == null)
                    candidate = temple.Owner;
                else
                {
                    if (candidate != temple.Owner)
                        return false;
                }
            }
            return true;
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
