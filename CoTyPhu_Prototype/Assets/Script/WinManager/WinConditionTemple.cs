using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public partial class PlotConstructionTemple { }
namespace WinCondition
{
    public class WinConditionTemple : WinCondition
    {
        // Which Player owns what temples
        public Dictionary<PlotConstructionTemple, Player> templeOwner;
        // The Win Splash screen to be shown when this victory is achieved.
        public GameObject WinScreen;

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
        static Action GetWinConCheckAction()
        {
            return new WinConCheckAction(GetInstance());
        }
    }
}
