using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deleter : MonoBehaviour
{
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.G))
        {
            Destroy(this.gameObject);
        }
    }
}
