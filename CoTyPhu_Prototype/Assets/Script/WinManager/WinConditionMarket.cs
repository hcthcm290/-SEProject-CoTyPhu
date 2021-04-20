using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public partial class PlotConstructionMarket { }
namespace WinCondition
{
    public class WinConditionMarket : IWinCondition
    {
        public Dictionary<PlotConstructionMarket, Player> MarketOwner;
        public List<List<PlotConstructionMarket>> Blocks;
        public GameObject WinScreen;

        static public WinConditionMarket GetInstance()
        {
            return Singleton<WinConditionMarket>.GetInstance();
        }
        public WinConditionMarket()
        {
            WinDescription =
                "Xây nhà, kinh doanh, phát triển kinh tế, bạn trở thành người được các chính phủ quý mến.\n" +
                "Hiểu được ước mơ “cao cả” của bạn, các chính phủ trên đã liên minh lại và phát động một cuộc chiến tranh trên toàn thế giới nhằm truyền bá “chủ nghĩa” của bạn ra các quốc gia “nghèo đói” kia.\n" +
                "Thế giới bị cuốn vào một vòng xoáy mới, gươm giáo, súng đạn và phép thuật nhắm vào nhau ở khắp nơi.\n" +
                "Một thời kỳ mới đến, nhiều điều thay đổi, tuy nhiên người nhiều tiền vẫn là kẻ thắng";
            WinName = "Mạng lưới rộng lớn";
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
