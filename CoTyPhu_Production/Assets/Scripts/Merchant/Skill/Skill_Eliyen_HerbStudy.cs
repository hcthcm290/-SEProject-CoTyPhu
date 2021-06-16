using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Eliyen_HerbStudy : BaseSkill
{
    [SerializeField] StatusHerbStudy _statusPrefab;

    public Skill_Eliyen_HerbStudy()
    {
        Set(
            name: "Herb Study",
            manacost: 3,
            currentmanacost: 3,
            description: "Receive a shield that IMMUNE to item or skill effect ONE time. Last ONE ROUND."
            );
    }

    public override bool CanActivate()
    {
        bool r = false;
        if (Owner.GetMana() >= CurrentManaCost)
            r = true;
        return r && base.CanActivate();
    }

    public override bool Activate()
	{
        if (CanActivate())
        {
            var status = Instantiate(_statusPrefab, Owner.transform);
            status.targetPlayer = Owner;
            status.StartListen();
            return base.Activate();
        }
        return false;
	}
}
