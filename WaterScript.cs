using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterScript : MonoBehaviour
{
    float waterCount;
    void Start()
    {
        waterCount = Random.Range(10f, 30f);
    }

    public float GetWater()
    {
        return waterCount;
    }

}
