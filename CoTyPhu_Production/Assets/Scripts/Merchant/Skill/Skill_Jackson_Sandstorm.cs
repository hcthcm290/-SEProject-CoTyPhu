using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Jackson_Sandstorm : BaseSkill
{
    [SerializeField] StatusSandStormOwner _statusPrefab;

    public Skill_Jackson_Sandstorm()
    {
        Set(
            name: "Sand storm army",
            manacost: 5,
            currentmanacost: 5,
            description: "Gain STATUS SANDSTORM to all MARKET PLOTS you PASS BY in this turn."
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
