using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // The target that the camera will follow
    public Transform target;

    // The distance between the camera and the target
    public Vector3 offset;

    // The speed with which the camera follows the target
    public float followSpeed = 10f;

    public float minDistance;
    void LateUpdate()
    {
        //if (Vector3.Distance(transform.position, target.position) < minDistance)
        //{
        //    return; // Stop following
        //}
        CameraTrasform();
        //Debug.Log(Vector3.Distance(transform.position, target.position));
    }

    void CameraTrasform()
    {
        // If there is no target, do nothing
        if (target == null) return;

        // Calculate the desired position
        Vector3 desiredPosition = target.position + offset;

        // Smoothly move the camera to the desired position
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, followSpeed * Time.deltaTime);
        transform.position = smoothedPosition;

        // Optionally, you can also set the camera to look at the target
        transform.LookAt(target);
    }
}
