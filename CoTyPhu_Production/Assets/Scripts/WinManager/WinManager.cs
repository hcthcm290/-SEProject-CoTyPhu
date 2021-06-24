using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        // this list container the turn count which player lose
        List<Pair<int, Player>> listPlayerEndTurn = new List<Pair<int, Player>>();

        public void NotifyPlayerLose(Player player)
        {
            if(listPlayerEndTurn.Find(x => x.Item2 == player) != null)
            {
                Debug.LogError("Get notified lose for player more than once");
                return;
            }

            int turnCount = TurnDirector.Ins.TurnCount;
            Pair<int, Player> playerEndTurn = new Pair<int, Player>(turnCount, player);
            listPlayerEndTurn.Add(playerEndTurn);
        }

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

            List<Player> wonPlayers = new List<Player>();
            WinCondition wonCondition = null;

            foreach (var winCondition in winConditions)
            {
                bool hasPlayerWon = false;

                foreach(var player in TurnDirector.Ins.ListPlayer)
                {
                    if (player.HasLost) continue;

                    bool won = winCondition.CheckWinner(player);
                    
                    if(won)
                    {
                        wonPlayers.Add(player);
                        hasPlayerWon = true;
                    }
                }

                if(hasPlayerWon)
                {
                    wonCondition = winCondition;
                    break;
                }
            }

            foreach(var player in wonPlayers)
            {
                player.Rank = 1;
            }


            int currentRank = TurnDirector.Ins.ListPlayer.Count;

            if (listPlayerEndTurn.Count > 0)
            {
                // Set rank for player lost

                listPlayerEndTurn.OrderBy(x => x.Item1); // Sort list player lose by the turn count they pass

                int currentTurnCount = listPlayerEndTurn[0].Item1;

                foreach (var playerEndTurn in listPlayerEndTurn)
                {
                    if (playerEndTurn.Item1 > currentTurnCount)
                    {
                        currentRank--;
                        currentTurnCount = playerEndTurn.Item1;
                    }

                    playerEndTurn.Item2.Rank = currentRank;
                }
                currentRank--;
            }

            // Set rank for player that hasn't lost but the winner appear
            foreach (var player in TurnDirector.Ins.ListPlayer)
            {
                if (wonPlayers.Contains(player) || player.HasLost) continue;

                player.Rank = currentRank;
            }

            wonCondition.ShowWinScreen();

            //ExitGame();
        }

        public void ExitGame()
        {
            // TODO: Kick the player to the main screen.
            // SceneManager.CreateScene("WinScreen");
            // SceneManager.LoadScene("WinScreen");
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