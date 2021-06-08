using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Merchant_Eliyen : BaseMerchant
{
    public BaseSkill skillScript;
    public BaseStatus passive;
    //public Merchant_Eliyen()
    //{
    //    Set(
    //        tagName: MerchantTag.Eliyen,
    //        name: "Young Girl Eliyen",
    //        skill: skillScript,
    //        passive: passive,
    //        maxMana: 8,
    //        story: "lady"
    //        );
    //}
    public override void Init()
    {
        Set(
            tagName: MerchantTag.Eliyen,
            name: "Young Girl Eliyen",
            skill: skillScript,
            passive: passive,
            maxMana: 8,
            story: ""
            );
        Lock();
    }
}
