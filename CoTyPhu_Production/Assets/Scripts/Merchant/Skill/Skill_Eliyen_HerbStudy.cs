using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Eliyen_HerbStudy : BaseSkill
{
    public Skill_Eliyen_HerbStudy()
    {
        Set(
            name: "Herb Study",
            manacost: 3,
            currentmanacost: 3,
            description: "Receive a shield that IMMUNE to item or skill effect ONE time. Last ONE ROUND."
            );
    }

    public override bool Activate()
	{
		return true;
	}
}
