using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public interface IDiceListener
{
    void OnRoll(int idPlayer, List<int> result);
    int GetDiceListenerPriority();
}
