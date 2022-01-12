using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Transform target;

    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, target.position, Time.deltaTime *2);
        transform.LookAt(target.parent.position + Vector3.up + Vector3.up);
    }
}
