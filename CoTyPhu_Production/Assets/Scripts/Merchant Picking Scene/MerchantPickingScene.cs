using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MerchantPicking
{
    public class MerchantPickingScene : MonoBehaviour
    {
        #region UI properties
        [SerializeField] GameObject listMerchantAvatarContent;
        [SerializeField] MerchantInfoCard merchantInfoCard;
        #endregion

        [SerializeField] List<BaseMerchant> listAvailableMerchant;
        [SerializeField] MerchantAvatar merchantAvatarPrefab;

        void Start()
        {
            foreach (var merchant in listAvailableMerchant)
            {
                MerchantAvatar merchantAvatar = Instantiate(merchantAvatarPrefab, listMerchantAvatarContent.transform);

                var _merchant = Instantiate(merchant, merchantAvatar.transform);
                _merchant.Init();
                _merchant.gameObject.SetActive(false);

                merchantAvatar.SetInfo(_merchant);
                merchantAvatar.onPress += OnMerchantAvatarPress;
            }
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void OnMerchantAvatarPress(BaseMerchant merchant)
        {
            merchantInfoCard.SetInfo(merchant);
        }
    }
}
