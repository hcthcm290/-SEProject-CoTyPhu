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
        [SerializeField] Image passiveSkillAvatar;
        [SerializeField] Text activeSkillName;
        [SerializeField] Text activeSkillDesc;
        [SerializeField] Image activeSkillAvatar;
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
            passiveSkillAvatar.sprite = merchant.PassiveSkill != null
                ? merchant.PassiveSkill.GetComponent<SpriteRenderer>().sprite
                : null;
            activeSkillName.text = merchant.Skill != null
                ? merchant.Skill.Name + $" ({merchant.Skill.ManaCost} mana)"
                : "";
            activeSkillDesc.text = merchant.Skill != null
                ? merchant.Skill.Description
                : "";
            activeSkillAvatar.sprite = merchant.Skill != null
                ? merchant.Skill.GetComponent<Image>().sprite
                : null;

        }

        public string GetMerchantName()
        {
            return merchantName.text;
        }
    }
}
