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
        if (TurnDirector.Ins.ListPlayer.Count < 2)
            return false;
        if (Owner.GetMana() >= CurrentManaCost)
            return true;
        return base.CanActivate();
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
