using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField]
    Transform target;
    [SerializeField]
    float smoothing = 0.6f; // How smoothly camera follows target.

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 targetPosition = new Vector3(target.position.x, target.position.y, target.position.z - 10);

        // Smoothly follow target.
        if(transform.position != target.position)
            transform.position = Vector3.Lerp(transform.position, targetPosition, smoothing);        
        
    }
}
