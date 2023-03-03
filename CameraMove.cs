using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    private void Start()
    {
    }
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            this.transform.position += Vector3.up * 0.25f;
        }
        if (Input.GetKey(KeyCode.S))
        {
            this.transform.position += Vector3.down * 0.25f;
        }
        if (Input.GetKey(KeyCode.D))
        {
            this.transform.position += Vector3.right * 0.25f;
        }
        if (Input.GetKey(KeyCode.A))
        {
            this.transform.position += Vector3.left * 0.25f;
        }

        if (Input.GetAxis("Mouse ScrollWheel") != 0f) // forward
        {
            if (this.GetComponent<Camera>().orthographicSize > 0)
            {
                this.GetComponent<Camera>().orthographicSize += Input.GetAxis("Mouse ScrollWheel") * 5;
            }
            else this.GetComponent<Camera>().orthographicSize += 1;
        }
    }
}
