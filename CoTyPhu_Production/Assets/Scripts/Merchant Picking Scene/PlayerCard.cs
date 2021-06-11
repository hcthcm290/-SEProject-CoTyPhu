using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;

namespace MerchantPicking
{
    public class PlayerCard : MonoBehaviour
    {
        #region UI properties
        [SerializeField] Image merchantAvatar;
        [SerializeField] Text playerName;
        [SerializeField] FloatingArrow _floatingArrow;
        #endregion

        private string clientID;

        private void Start()
        {
        }

        public void SetInfo(BaseMerchant merchant)
        {
            merchantAvatar.sprite = merchant.GetComponent<Image>().sprite;
        }

        public void SetPlayer(Photon.Realtime.Player player)
        {
            playerName.text = player.NickName;
            clientID = player.UserId;
        }

        public string GetClientID()
        {
            return clientID;
        }

        public void EnableFloatingArrow()
        {
            _floatingArrow.gameObject.SetActive(true);
            _floatingArrow.enabled = true;
        }

        public void DisableFloatingArrow()
        {
            _floatingArrow.enabled = false;
        }
    }
}
