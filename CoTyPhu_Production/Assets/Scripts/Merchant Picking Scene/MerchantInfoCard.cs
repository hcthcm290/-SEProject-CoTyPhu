using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MerchantPicking
{
    public class MerchantInfoCard : MonoBehaviour
    {
        #region UI properties
        [SerializeField] Image merchantAvatar;
        [SerializeField] Text merchantName;
        [SerializeField] Text passiveSkillName;
        [SerializeField] Text passiveSkillDesc;
        [SerializeField] Text activeSkillName;
        [SerializeField] Text activeSkillDesc;
        #endregion

        private void Start()
        {
            merchantName.text = "";
            passiveSkillName.text = "";
            passiveSkillDesc.text = "";
            activeSkillName.text = "";
            activeSkillDesc.text = "";
        }

        public void SetInfo(BaseMerchant merchant)
        {
            merchantAvatar.sprite = merchant.GetComponent<Image>().sprite;
            merchantName.text = merchant.Name;
            passiveSkillName.text = merchant.PassiveSkill != null
                ? merchant.PassiveSkill.Name + " (passive)"
                : "";
            passiveSkillDesc.text = merchant.PassiveSkill != null
                ? merchant.PassiveSkill.Description
                : "";
            activeSkillName.text = merchant.Skill != null
                ? merchant.Skill.Name + $" ({merchant.Skill.ManaCost} mana)"
                : "";
            activeSkillDesc.text = merchant.Skill != null
                ? merchant.Skill.Description
                : "";

        }

        public string GetMerchantName()
        {
            return merchantName.text;
        }
    }
}
