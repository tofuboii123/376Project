using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField]
    float speed = 5.0f;

    [SerializeField]
    bool inPast = false;

    // Update is called once per frame
    void Update()
    {

        // Time travel.
        if (Input.GetButtonDown("TimeShift"))
            TimeTravel();
        
        Vector2 direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        this.transform.Translate(direction.normalized * speed * Time.deltaTime);
    }

    void TimeTravel() {

        float travel = 0;

        // Go from past to present.
        if (!inPast) {
            travel = 15;
            inPast = true;
        }
        else {
            travel = -15;
            inPast = false;
        }

        this.transform.position += new Vector3(0, 0, travel); // Unity doesn't allow you to only modify the z value...
    }
}
