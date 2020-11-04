using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clock : MonoBehaviour
{
    private Transform arm;

    private static int direction = -1;
    private static float position;
    private static float rotations;
    private static float increments;

    void Awake()
    {
        arm = transform.Find("Arm");
    }

    void Update()
    {
        if (position <= rotations) {
            arm.rotation = Quaternion.Euler(0, 0, direction * position);

            position = Mathf.Round(position + increments);

            if (position > 720) {
                position = 720;
            }
            
            increments *= 1.0025f;
        }
    }

    public static void TimeTravel()
    {
        position = 0;
        direction *= -1;
        rotations = 720;
        increments = 1;
    }
}
