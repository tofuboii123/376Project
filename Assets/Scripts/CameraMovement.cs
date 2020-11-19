using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField]
    Transform target;
    [SerializeField]
    float smoothing = 0.6f; // How smoothly camera follows target.
    public bool cutscene_mode = false;
    // Update is called once per frame
    void LateUpdate()
    {
        if (cutscene_mode == false)
        {
            Vector3 targetPosition = new Vector3(target.position.x, target.position.y, target.position.z - 10);

            // Smoothly follow target.
            if (transform.position != target.position)
                transform.position = Vector3.Lerp(transform.position, targetPosition, smoothing);

        }
    }
}
