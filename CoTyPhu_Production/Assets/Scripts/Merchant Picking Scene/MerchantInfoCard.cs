using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MerchantInfoCard : MonoBehaviour
{
    #region UI properties
    [SerializeField] Image merchantAvatar;
    [SerializeField] Text merchantName;
    [SerializeField] Text passiveSkillName;
    [SerializeField] Text passivesKillDesc;
    [SerializeField] Text activeSkillName;
    [SerializeField] Text activeSkillDesc;
    #endregion

    public void SetInfo(BaseMerchant merchant)
    {
        merchantAvatar.sprite = merchant.GetComponent<Image>().sprite;
        merchantName.text = merchant.Name;
        passiveSkillName.text = merchant.PassiveSkill != null 
            ? merchant.PassiveSkill.Name + " (passive)" 
            : "";
        passivesKillDesc.text = merchant.PassiveSkill != null 
            ? merchant.PassiveSkill.Description 
            : "";
        activeSkillName.text = merchant.Skill != null
            ? merchant.Skill.Name + $" ({merchant.Skill.ManaCost} mana)" 
            : "";
        activeSkillDesc.text = merchant.Skill != null 
            ? merchant.Skill.Description 
            : "";

    }
}
