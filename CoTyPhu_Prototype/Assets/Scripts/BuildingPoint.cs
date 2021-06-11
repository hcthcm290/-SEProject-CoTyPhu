using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingPoint : MonoBehaviour
{
    public int currentHouseID; //id == -1 means no house is built on the point
    GameObject currentHouse;

    [SerializeField]
    Transform buildPoint;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    int GetKeyNumber()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0)) return 0;
        if (Input.GetKeyDown(KeyCode.Alpha1)) return 1;
        if (Input.GetKeyDown(KeyCode.Alpha2)) return 2;
        if (Input.GetKeyDown(KeyCode.Alpha3)) return 3;
        if (Input.GetKeyDown(KeyCode.Alpha4)) return 4;
        return currentHouseID;
    }

    // Update is called once per frame
    void Update()
    {
        //demo: make key_catch to build house
        int newHouse = GetKeyNumber();
        if (newHouse != currentHouseID)
        {
            if (currentHouse != null) Destroy(currentHouse.gameObject);
            currentHouse = Instantiate(BuildingLibrary.sBuildings[newHouse], buildPoint.position, Quaternion.identity);
            currentHouseID = newHouse;
        }
    }

    public void Build(int type)
    {
        if (currentHouse != null) Destroy(currentHouse.gameObject);
        currentHouse = Instantiate(BuildingLibrary.sBuildings[type], buildPoint.position, buildPoint.rotation);
        currentHouseID = type;
        currentHouse.transform.parent = this.transform;
    }
}
