using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    float speed = 5.0f;

    [SerializeField]
    bool inPast = false;

    [SerializeField]
    float travel = 150.0f; // Distance of 2nd timeline in y

    // Update is called once per frame
    void Update()
    {
        // Time travel.
        if (Input.GetButtonDown("TimeShift"))
            TimeShift();

        MovePlayer();
    }

    void MovePlayer() {
        Vector2 direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        this.transform.Translate(direction.normalized * speed * Time.deltaTime);
    }

    // Go from past to present and vice-versa
    void TimeShift() {

        // Boolean switch
        if (!inPast)
            inPast = true;
        else
            inPast = false;
        
        this.transform.position += new Vector3(0, (inPast ? travel : -travel), 0); // Unity doesn't allow you to only modify the z value...
    }

}
