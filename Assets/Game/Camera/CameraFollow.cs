using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour
{
    public Transform target;

    float smoothing = .3f; //.5 means midway between frame a and b, the lower the number, the more smoothing

    public float fov = 30;
    private float previousfov;

    public bool isFollowing = false;

    public Vector3 cameraPosTranslation;
    
    void Start()
    {
        transform.SetParent(target, false);
    }

    void LateUpdate()
    {
        transform.position = new Vector3(0, transform.position.y, transform.position.z);
    }

    /*void FixedUpdate()
    {
        if(target == null)
        {
            if(GameManager.instance.player != null)
            {
                target = GameManager.instance.player.transform;
            }
        }

        if (isFollowing && target != null)
        {
            if (Camera.main.fieldOfView != fov)
            {
                previousfov = Camera.main.fieldOfView;
                Camera.main.fieldOfView = fov;
            }
            
            transform.position = target.position + cameraPosTranslation;

            //transform.position = Vector3.Lerp(transform.position, target.position + cameraPosTranslation, smoothing);
            //transform.rotation = Quaternion.Lerp(transform.rotation, targetCameraRotation, smoothing);
        }
    }*/
}
