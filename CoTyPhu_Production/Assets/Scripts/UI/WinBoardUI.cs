using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WinBoardUI : MonoBehaviour
{
    [System.Serializable]
    private class RawMerchantPrefabPair
    {
        public MerchantTag tag;
        public GameObject merchant;
    }
    #region UI properties
    [SerializeField] Image background;
    [SerializeField] Image rankImage;
    [SerializeField] TextMeshProUGUI winTitle;
    [SerializeField] TextMeshProUGUI winDescription;
    [SerializeField] List<PlayerResultCard> playerResultCards;
    [SerializeField] List<RawMerchantPrefabPair> rawMerchantResources;
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
        }
    }
}
