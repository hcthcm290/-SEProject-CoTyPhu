using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using System.Linq;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using UnityEngine.SceneManagement;

namespace MerchantPicking
{
    public class MerchantPickingScene : MonoBehaviourPun
    {
        #region UI properties
        [SerializeField] GameObject listMerchantAvatarContent;
        [SerializeField] MerchantInfoCard merchantInfoCard;
        [SerializeField] GameObject listPlayerCardContent;
        [SerializeField] Button lockButton;
        [SerializeField] TextMeshProUGUI countDownText;
        #endregion

        [SerializeField] List<BaseMerchant> listAvailableMerchant;
        [SerializeField] List<BaseMerchant> listInstantMerchant;
        [SerializeField] MerchantAvatar merchantAvatarPrefab;
        [SerializeField] PlayerCard playerCardPrefab;

        [SerializeField] List<PlayerCard> playerCards;

        [SerializeField] List<Photon.Realtime.Player> listPlayer;

        int currentTurnPlayerIndex = 0;
        int playersCount = 0;

        void Start()
        {
            listInstantMerchant = new List<BaseMerchant>();
            foreach (var merchant in listAvailableMerchant)
            {
                MerchantAvatar merchantAvatar = Instantiate(merchantAvatarPrefab, listMerchantAvatarContent.transform);

                var _merchant = Instantiate(merchant, merchantAvatar.transform);
                _merchant.Init();
                _merchant.gameObject.SetActive(false);

                listInstantMerchant.Add(_merchant);

                merchantAvatar.SetInfo(_merchant);
                merchantAvatar.onPress += OnMerchantAvatarPress;
            }

            foreach(var player in PhotonNetwork.CurrentRoom.Players)
            {
                var playerCard = Instantiate(playerCardPrefab, listPlayerCardContent.transform);
                playerCard.SetPlayer(player.Value);

                playerCards.Add(playerCard);
            }

            photonView.RPC("Ready", PhotonNetwork.MasterClient, PhotonNetwork.LocalPlayer.UserId);
        }

        public void OnMerchantAvatarPress(BaseMerchant merchant)
        {
            merchantInfoCard.SetInfo(merchant);
        }

        private void Begin()
        {
            if (!PhotonNetwork.IsMasterClient) return;

            Debug.Log("Game started");

            Debug.Log("prepare to shuffle the player list");

            List<int> testList = new List<int>();
            for(int i=0; i<10; i++)
            {
                testList.Add(i);
            }
            testList.Shuffle();

            listPlayer = new List<Photon.Realtime.Player>(PhotonNetwork.PlayerList);
            listPlayer.Shuffle();

            Debug.Log("prepare to assign id based on index in list");
            int index = 0;
            foreach(var player in listPlayer)
            {
                ExitGames.Client.Photon.Hashtable userIdProperties = new ExitGames.Client.Photon.Hashtable();
                userIdProperties.Add("id", index);

                player.SetCustomProperties(userIdProperties);

                index++;
            }

            Debug.Log("finish assign id to player");
            Debug.Log("set turn");
            string nextClientID = listPlayer[currentTurnPlayerIndex].UserId;
            photonView.RPC("BeginTurn", RpcTarget.AllBufferedViaServer, nextClientID);
        }

        public void Lock()
        {
            if(merchantInfoCard.GetMerchantName() != "")
            {
                string clientID = PhotonNetwork.LocalPlayer.UserId;
                string merchantName = merchantInfoCard.GetMerchantName();
                photonView.RPC("LockMerchant", RpcTarget.AllBufferedViaServer, clientID, merchantName);
            }
        }

        #region Photon
        [PunRPC]
        private void Ready(string clientID)
        {
            playersCount++;
            if(playersCount == PhotonNetwork.CurrentRoom.PlayerCount)
            {
                Begin();
            }
        }

        [PunRPC]
        private void LockMerchant(string clientID, string merchantname)
        {
            var merchantPrefab = listInstantMerchant.Find(x => x.Name == merchantname);
            var playerCard = playerCards.Find(x => x.GetClientID() == clientID);

            var _merchant = Instantiate(merchantPrefab, playerCard.transform);
            _merchant.Init();
            _merchant.gameObject.SetActive(false);

            playerCard.SetInfo(_merchant);

            if(PhotonNetwork.IsMasterClient)
            {
                // Set player properties for choosen merchant
                Photon.Realtime.Player player = PhotonNetwork.PlayerList.First(x => x.UserId == clientID);
                ExitGames.Client.Photon.Hashtable merchantProperties = new ExitGames.Client.Photon.Hashtable();
                merchantProperties.Add("merchantType", (int)(merchantPrefab.TagName));
                player.SetCustomProperties(merchantProperties);

                currentTurnPlayerIndex++;

                if(currentTurnPlayerIndex < listPlayer.Count)
                {
                    string nextClientID = listPlayer[currentTurnPlayerIndex].UserId;
                    photonView.RPC("BeginTurn", RpcTarget.AllBufferedViaServer, nextClientID);
                }
                else
                {
                    photonView.RPC("MoveToPlayScene", RpcTarget.AllBufferedViaServer);

                    photonView.RPC("BeginTurn", RpcTarget.AllBufferedViaServer, "-1");
                }
            }
        }

        [PunRPC]
        private void MoveToPlayScene()
        {
            int curCd = 4;
            Tweener currentTweener = null;

            countDownText.text = "Game Started";
            countDownText.transform.localScale = new Vector3(2, 2, 2);
            currentTweener = countDownText.transform.DOScale(4, 2.5f);

            currentTweener.onComplete = () =>
            {
                curCd--;

                if (curCd < 0)
                {
                    // Todo: move to play scene
                    foreach(var player in PhotonNetwork.PlayerList)
                    {
                        Debug.Log($"{player.NickName}, id: {player.CustomProperties["id"] as int?}, " +
                            $"merchant: {(MerchantTag)(player.CustomProperties["merchantType"] as int?)}");
                    }
                    SceneManager.LoadScene("SampleScene");
                    return;
                }

                Debug.Log(curCd);
                countDownText.text = curCd.ToString();
                var color = countDownText.color;
                color.a = 1;
                countDownText.color = color;
                countDownText.transform.localScale = new Vector3(3, 3, 3);
                countDownText.transform.DOScale(10, 1);
                var newTweener = countDownText.DOFade(0, 1);
                newTweener.onComplete = currentTweener.onComplete;
                currentTweener = newTweener;
            };
        }

        [PunRPC]
        public void BeginTurn(string clientID)
        {
            //var client = PhotonNetwork.PlayerList.FirstOrDefault(x => x.UserId == clientID);

            if(PhotonNetwork.LocalPlayer.UserId == clientID)
            {
                lockButton.gameObject.SetActive(true);
            }
            else
            {
                lockButton.gameObject.SetActive(false);
            }

            foreach(var playerCard in playerCards)
            {
                if(playerCard.GetClientID() == clientID)
                {
                    playerCard.EnableFloatingArrow();
                }
                else
                {
                    playerCard.DisableFloatingArrow();
                }
            }
        }
        #endregion
    }
}
