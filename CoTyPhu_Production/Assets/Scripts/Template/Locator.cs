using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Locator<T> where T:class
{
    static public T Instance = null;
}
// Locate Marked instance
public class Locator
{
    static public void MarkInstance<T>(T item) where T:class
    {
        Locator<T>.Instance = item;
    }
    static public T GetInstance<T>() where T : class
    {
        return Locator<T>.Instance;
    }
}
// Inherit this / call GetInstance to mark as singleton
public class Singleton<T> where T : class, new()
{
    static public T GetInstance()
    {
        var result = Locator.GetInstance<T>();
        if (result == null)
        {
            result = new T();
            Locator.MarkInstance<T>(result);
        }
        return result;
    }
}