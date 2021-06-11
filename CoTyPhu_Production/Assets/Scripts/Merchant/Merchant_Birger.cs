using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Merchant_Birger : BaseMerchant
{
    public BaseSkill skillScript;
    public BaseStatus passive;

    public override void Init()
    {
        Set(
            tagName: MerchantTag.Jackson,
            name: "Oldman Birger",
            skill: skillScript,
            passive: passive,
            maxMana: 8,
            story: ""
            );
        Lock();
    }
}
