using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public abstract class Skill
{
    int _currentManaCost;
    Player _player;

    public void SetPlayer(Player player)
    {
        _player = player;
    }
    public abstract bool CanActivateSkill();
    public abstract void ActivateSkill();
    public abstract string GetName();
    public abstract string GetDescription();
}
