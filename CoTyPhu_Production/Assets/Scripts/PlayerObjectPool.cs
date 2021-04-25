using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerObjectPool : MonoBehaviour
{
    public static PlayerObjectPool Ins;

    private void Start()
    {
        Ins = this;
    }

    [SerializeField] public List<GameObject> PlayersPool;
}
