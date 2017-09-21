using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Unit
{    
    void Update()
    {
        var dir = new Vector3(0, 0, 1);
        transform.position += dir * speed * Time.deltaTime;
        anim.SetFloat("Speed", speed);
    }
}
