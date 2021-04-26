using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Status { }
public abstract class Merchant
{
    protected Player _player;
    protected Status _passiveSkill;
    protected Skill _activeSkill;
    protected string _name;
    public void SetPlayer(Player player)
    {
        _player = player;
    }
    public Player GetPlayer() => _player;
    public string GetName() => _name;
    public abstract string GetTagName();
    public Status GetPassiveSkill() => _passiveSkill;
    public Skill GetActiveSkill() => _activeSkill;
}
