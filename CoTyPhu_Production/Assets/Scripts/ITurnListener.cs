using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITurnListener
{
    void OnBeginTurn(int idPlayer);
    void OnEndTurn(int idPlayer);
}
