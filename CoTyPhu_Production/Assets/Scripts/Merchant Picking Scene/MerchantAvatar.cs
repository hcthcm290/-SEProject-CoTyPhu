using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MerchantPicking
{

    public class MerchantAvatar : MonoBehaviour
    {
        #region UI properties
        [SerializeField] Image merchantAvatar;
        #endregion

        [SerializeField] BaseMerchant merchantInfo;

        public delegate void OnPressFunction(BaseMerchant merchant);
        public event OnPressFunction onPress;

        public void SetInfo(BaseMerchant merchant)
        {
            merchantInfo = merchant;

            merchantAvatar.sprite = merchantInfo.GetComponent<Image>().sprite;
        }

        public void OnPress()
        {
            onPress?.Invoke(merchantInfo);
        }
    }
}

