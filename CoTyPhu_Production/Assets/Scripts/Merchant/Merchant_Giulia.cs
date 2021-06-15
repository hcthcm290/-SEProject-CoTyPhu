using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Merchant_Giulia : BaseMerchant
{
    public BaseSkill skillScript;
    public BaseStatus passive;

    public override void Init()
    {
        Set(
            tagName: MerchantTag.Giulia,
            name: "Devil Giulia",
            skill: skillScript,
            passive: passive,
            maxMana: 10,
            story: ""
            );
        Lock();
    }
}
