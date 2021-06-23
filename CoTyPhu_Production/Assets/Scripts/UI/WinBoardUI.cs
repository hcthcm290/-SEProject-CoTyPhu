using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class WinBoardUI : MonoBehaviour
{
    [System.Serializable]
    private class RawMerchantPrefabPair
    {
        public MerchantTag tag;
        public GameObject merchant;
        public Sprite splashScreen;
    }
    #region UI properties
    [SerializeField] Image background;
    [SerializeField] Image rankImage;
    [SerializeField] TextMeshProUGUI winTitle;
    [SerializeField] TextMeshProUGUI winDescription;
    [SerializeField] List<PlayerResultCard> playerResultCards;
    [SerializeField] List<RawMerchantPrefabPair> rawMerchantResources;
    [SerializeField] List<Sprite> rankSprites;
    [SerializeField] GameObject WinDescriptionBoard;
    #endregion

    private void OnEnable()
    {
        foreach(var playerResultCard in playerResultCards)
        {
            playerResultCard.gameObject.SetActive(false);
        }

        List<Player> finalListPlayer = new List<Player>(TurnDirector.Ins.ListPlayer);
        finalListPlayer.OrderBy(player => player.Rank);
        for (int i = 0; i < finalListPlayer.Count; i++)
        {
            var player = finalListPlayer[i];
            playerResultCards[i].gameObject.SetActive(true);

            var rawMerchantPrefab = rawMerchantResources.Find(x => x.tag == player.GetMerchant().TagName).merchant;
            playerResultCards[i].SetInfo(player, rawMerchantPrefab);

            if(player.MinePlayer)
            {
                rankImage.sprite = rankSprites[player.Rank - 1];
            }

            if(player.Rank == 1)
            {
                background.sprite = rawMerchantResources.Find(x => x.tag == player.GetMerchant().TagName).splashScreen;
            }
        }
    }

    public void SetWinDescription(string title, string description)
    {
        winTitle.text = title;
        winDescription.text = description;
    }

    public void ToggleWinDescriptionOnOff()
    {
        WinDescriptionBoard.SetActive(!WinDescriptionBoard.activeSelf);
    }

    public void Continue()
    {
        SceneManager.LoadScene("WaitingRoomScene");
    }
}
