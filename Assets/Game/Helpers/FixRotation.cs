using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

public class FixRotation : MonoBehaviour
{
    Quaternion rotation;

    float height;
    void Awake()
    {
        //rotation = transform.rotation;
        height = transform.position.y;
    }
    void LateUpdate()
    {
        //transform.rotation = rotation;
        transform.position = new Vector3(transform.position.x, height, transform.position.z);
    }
}
