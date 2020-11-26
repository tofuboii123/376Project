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

        if (instance.gameObject.activeInHierarchy) {
            instance.StartCoroutine(instance.StartClockHandRotation());
        }
    }

    IEnumerator StartClockHandRotation() {
        float startZRotation = arm.eulerAngles.z;
        float endZRotation = (direction == 1 ? startZRotation + 720.0f : startZRotation - 720.0f);

        float timer;
        float zRotation;

        timer = 0.0f;
        while (timer < 1.0f) {
            timer += Time.deltaTime;

            zRotation = Mathf.Lerp(startZRotation, endZRotation, Mathf.Sin(((timer / 1.0f) * 0.5f) * Mathf.PI)) % 360.0f;
            arm.eulerAngles = new Vector3(arm.eulerAngles.x, arm.eulerAngles.y, zRotation);

            yield return null;
        }

        // Make sure that the arm is aligned correctly
        arm.eulerAngles = new Vector3(arm.eulerAngles.x, arm.eulerAngles.y, endZRotation);
    }
}
