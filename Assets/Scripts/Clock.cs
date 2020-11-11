using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clock : MonoBehaviour
{
    private Transform arm;

    private static Clock instance;

    private static int direction;

    void Awake()
    {
        arm = transform.Find("Arm");
        instance = this; // For starting coroutines from a static method
        direction = -1;
    }

    public static void TimeTravel()
    {
        direction *= -1;

        instance.StartCoroutine(instance.StartClockHandRotation());
    }

    IEnumerator StartClockHandRotation() {
        float startRotation = arm.eulerAngles.z;
        float endRotation = (direction == 1 ? startRotation + 720.0f : startRotation - 720.0f);

        float t = 0.0f;

        while (t < 1.0f) {
            t += Time.deltaTime;
            float zRotation = Mathf.Lerp(startRotation, endRotation, Mathf.Sin(((t / 1.0f) * 0.5f) * Mathf.PI)) % 360.0f;
            arm.eulerAngles = new Vector3(arm.eulerAngles.x, arm.eulerAngles.y, zRotation);
            yield return null;
        }
    }
}
