using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabContainer : MonoBehaviour
{
    static public PrefabContainer Ins 
    { 
        get => Locator.GetInstance<PrefabContainer>(); 
    }

    PrefabContainer()
    {
        Locator.MarkInstance(this);
    }


    public MagicEffect magicOrb;
    public MosesStaff mosesStaff;

}
