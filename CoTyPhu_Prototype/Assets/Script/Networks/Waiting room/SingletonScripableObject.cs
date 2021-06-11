using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonScripableObject<T> : ScriptableObject where T : ScriptableObject
{
    private static T _instance;

    public static T instance
    {
        get
        {
            if(_instance == null)
            {
                T[] results = Resources.FindObjectsOfTypeAll<T>();
                _instance = results[0];
            }

            return _instance;
        }
    }
}
