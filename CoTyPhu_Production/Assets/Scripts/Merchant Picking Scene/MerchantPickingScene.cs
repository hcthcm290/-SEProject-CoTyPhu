using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using System.Linq;
using UnityEngine.UI;

namespace MerchantPicking
{
    public class MerchantPickingScene : MonoBehaviourPun
    {
        #region UI properties
        [SerializeField] GameObject listMerchantAvatarContent;
        [SerializeField] MerchantInfoCard merchantInfoCard;
        [SerializeField] GameObject listPlayerCardContent;
        [SerializeField] Button lockButton;
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
                player.CustomProperties.Add("id", index);
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
                currentTurnPlayerIndex++;

                if(currentTurnPlayerIndex < listPlayer.Count)
                {
                    string nextClientID = listPlayer[currentTurnPlayerIndex].UserId;
                    photonView.RPC("BeginTurn", RpcTarget.AllBufferedViaServer, nextClientID);
                }
            }
        }

        [PunRPC]
        public void BeginTurn(string clientID)
        {
            var client = PhotonNetwork.PlayerList.First(x => x.UserId == clientID);
            Debug.Log("Begin turn " + client.NickName);

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
