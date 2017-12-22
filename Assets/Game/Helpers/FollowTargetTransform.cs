using UnityEngine;
using System.Collections;

public class FollowTargetTransform : MonoBehaviour
{
    public Transform target;

    Vector3 diff;

    void Start()
    {
        diff = target.position - transform.position;
    }

    void LateUpdate()
    {
        transform.position = target.position - diff;
    }
}
