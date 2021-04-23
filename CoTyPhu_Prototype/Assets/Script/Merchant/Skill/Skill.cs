using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public abstract class Skill
{
    int _currentManaCost;
    Player _player;
    // Set the current affected player.
    public void SetPlayer(Player player)
    {
        _player = player;
    }
    // Whether the skill can be activated (mana condition, special condition, ...)
    public abstract bool CanActivateSkill();
    // Activate the skill. Creates the Action of the ActiveSkill.
    public abstract Action ActivateSkill();
    // Return the ActiveSkill's name.
    public abstract string GetName();
    // Return the ActiveSkill's description
    public abstract string GetDescription();
}
