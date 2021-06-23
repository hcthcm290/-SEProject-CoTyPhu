using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Giulia_Enchant : BaseSkill
{
    [SerializeField] StatusTagByGiulia prefab; 
    public Skill_Giulia_Enchant()
    {
        Set(
            name: "Enchant Spell",
            manacost: 5,
            currentmanacost: 5,
            description: "Bring OTHER player to your current PLOT."
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
            PlayerObjectPool playerPool;
            if (PlayerObjectPool.Ins == null)
            {
                playerPool = GameObject.Find("PlayerPool").GetComponent<PlayerObjectPool>();
            }
            else
            {
                playerPool = PlayerObjectPool.Ins;
            }

            Player myPlayer = null;
            List<Player> otherPlayers = new List<Player>();
            for (int i = 0; i < playerPool.transform.childCount; i++)
            {
                Player p = playerPool.transform.GetChild(i).GetComponent<Player>();
                if(p.Id == Owner.Id)
                {
                    myPlayer = p;
                }
                else
                {
                    if (p.gameObject.activeSelf == true)
                    {
                        otherPlayers.Add(p);
                    }
                }
            }

            Player targetPlayer = ClosestPlayer(myPlayer, otherPlayers);
            if (targetPlayer != null)
            {
                targetPlayer.ActionMoveTo(Owner.Location_PlotID).PerformAction();
                //Plot.plotDictionary[Owner.Location_PlotID].ActiveOnEnter(targetPlayer);

                var status = Instantiate(prefab, targetPlayer.transform);
                status.targetPlayer = targetPlayer;
                status.Owner = Owner;
                status.StartListen();
            }
            return base.Activate();
        }
        return false;
    }

    public Player ClosestPlayer(Player my, List<Player> others)
    {
        Player result = my;
        int com = 32;
        foreach(Player o in others)
        {
            int dis = 32;

            if(o.Location_PlotID >= my.Location_PlotID)
            {
                dis = 32 + my.Location_PlotID - o.Location_PlotID;
            }
            else
            {
                dis = my.Location_PlotID - o.Location_PlotID;
            }

            if(dis <= com)
            {
                result = o;
                com = dis;
            }
        }
        return result != my? result : null;
    }
}
