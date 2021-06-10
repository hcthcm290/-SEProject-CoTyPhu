using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Merchant_Jackson : BaseMerchant
{
    public BaseSkill skillScript;
    public BaseStatus passive;

    public override void Init()
    {
        Set(
            tagName: MerchantTag.Jackson,
            name: "Young Merchant Jackson",
            skill: skillScript,
            passive: passive,
            maxMana: 8,
            story: ""
            );
        Lock();
    }
}
