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

    [System.Serializable]
    private class HouseFamily
    {
        public MerchantTag tag;
        [SerializeField] public List<HousePrefab> _listHousePrefab;
    }
    [SerializeField] List<HouseFamily> _listHouseFamily;
    #endregion

    #region methods
    public GameObject GetPrefab(MerchantTag tag, int level)
    {
        var houseFamily = _listHouseFamily.Find(x => x.tag == tag);

        if (houseFamily == null) return null;

        var housePrefab = houseFamily._listHousePrefab.Find(x => x.level == level);

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
