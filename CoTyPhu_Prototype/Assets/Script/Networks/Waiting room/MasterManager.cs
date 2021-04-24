using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Singleton/MasterManager")]
public class MasterManager : SingletonScripableObject<MasterManager>
{
    [SerializeField]
    private GameManager _gameManager;
    public static GameManager GameManager { get { return instance._gameManager; } }
}
