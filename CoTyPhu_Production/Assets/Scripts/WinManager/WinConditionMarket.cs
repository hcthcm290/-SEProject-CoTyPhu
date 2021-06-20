using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace WinCondition
{
    public class WinConditionMarket : WinCondition
    {
        public List<List<PLOT>> Blocks = new List<List<PLOT>>
        {
            new List<PLOT>{PLOT.A1, PLOT.A2},
            new List<PLOT>{PLOT.B1, PLOT.B2},
            new List<PLOT>{PLOT.C1, PLOT.C2, PLOT.C3},
            new List<PLOT>{PLOT.D1, PLOT.D2},
            new List<PLOT>{PLOT.E1, PLOT.E2},
            new List<PLOT>{PLOT.F1, PLOT.F2, PLOT.F3},
            new List<PLOT>{PLOT.G1, PLOT.G2},
            new List<PLOT>{PLOT.H1, PLOT.H2},
        };
        public const int blocksOwnedToWin = 3;
        // The Win Splash screen to be shown when this victory is achieved.
        public WinBoardUI WinScreen;

        // Get Singleton Instance
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
            Locator.MarkInstance(this);
        }
        public override bool CheckWinner(Player candidate = null)
        {
            Dictionary<Player, int> blockOwned = new Dictionary<Player, int>();
            foreach (var block in Blocks)
            {
                Player current = candidate;
                bool isBlockOwned = true;

                foreach (PLOT plot in block)
                {
                    Player owner = (Plot.plotDictionary[plot] as PlotConstructionMarket).Owner;

                    if (owner == null)
                        isBlockOwned = false;
                    else if (current == null)
                        current = owner;
                    else if (current != owner)
                        isBlockOwned = false;

                    if (!isBlockOwned)
                        break;
                }

                if (isBlockOwned)
                {
                    if (blockOwned.ContainsKey(current))
                        blockOwned[current]++;
                    else
                        blockOwned[current] = 1;
                }
            }

            return blockOwned.Values.Any(amount => amount >= blocksOwnedToWin);
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
