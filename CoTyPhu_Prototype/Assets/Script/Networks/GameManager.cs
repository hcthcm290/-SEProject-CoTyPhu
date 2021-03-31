using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Manager/GameManager")]
public class GameManager : ScriptableObject
{
    [SerializeField]
    private string _gameVersion;
    public string gameVersion { get { return _gameVersion; } }

    [SerializeField]
    private string _nickName;

    public string NickName
    {
        get { return _nickName; }
        set { _nickName = value; }
    }
}
