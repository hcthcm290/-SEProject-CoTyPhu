using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingLibrary : MonoBehaviour
{
    public static List<GameObject> sBuildings;

    [SerializeField]
    List<GameObject> buildings;

    // Start is called before the first frame update
    void Start()
    {
        sBuildings = buildings;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
