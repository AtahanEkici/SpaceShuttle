using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public Transform target;
    public Transform cam;
    public float smoothSpeed = 0.125f;
    public Vector3 offset;
    Quaternion targetRotation;

    // Start is called before the first frame update
    void Start()
    {
        targetRotation = target.rotation;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        targetRotation = Quaternion.LookRotation(smoothedPosition);
        transform.position = smoothedPosition;
        //transform.LookAt(target);
    }
}
