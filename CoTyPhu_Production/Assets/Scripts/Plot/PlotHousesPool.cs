using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlotHousesPool : MonoBehaviour
{
    #region Singleton
    private static PlotHousesPool _ins;
    public static PlotHousesPool Ins
    {
        get
        {
            if(_ins == null)
            {
                Debug.LogWarning("Plot House Pool Ins is null");
            }
            return _ins;
        }
    }
    #endregion

    #region Properties
    [System.Serializable]
    private class HousePrefab
    {
        public int level;
        public GameObject prefab;
    }

    [SerializeField] List<HousePrefab> _listHousePrefab;
    #endregion

    #region methods
    public GameObject GetPrefab(int level)
    {
        var housePrefab = _listHousePrefab.Find(x => x.level == level);

        if (housePrefab != null)
        {
            return housePrefab.prefab;
        }

        return null;
    }
    #endregion

    #region Unity methods
    private void Start()
    {
        _ins = this;
    }
    #endregion
}
