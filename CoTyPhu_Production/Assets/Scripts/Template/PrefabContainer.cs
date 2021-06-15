using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabContainer : MonoBehaviour
{
    static public PrefabContainer Ins { get => Locator.GetInstance<PrefabContainer>(); }
    public MagicEffect magicOrb;

    PrefabContainer()
    {
        Locator.MarkInstance(this);
    }
}
