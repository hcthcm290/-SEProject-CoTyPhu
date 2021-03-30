using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Manager/GameManager")]
public class GameManager : ScriptableObject
{
    [SerializeField]
    private string _gameVersion;
    public string gameVersion { get { return _gameVersion; } }
}
