using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static bool canMove = true;

    [SerializeField]
    float speed = 5.0f;

    [SerializeField]
    bool inPast = false;

    [SerializeField]
    float travel = 150.0f; // Distance of 2nd timeline in y

    private Vector2 movement;

    public Animator animator;

    // Update is called once per frame
    void Update()
    {
        // Time travel.
        if (Input.GetButtonDown("TimeShift"))
            TimeShift();

        MovePlayer();
    }

    void MovePlayer() {
        if (canMove)
        {
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");

            if (movement != Vector2.zero) {
                animator.SetFloat("Horizontal", movement.x);
                animator.SetFloat("Vertical", movement.y);
            }

            animator.SetFloat("Speed", movement.sqrMagnitude);

            this.transform.Translate(movement.normalized * speed * Time.deltaTime);
        } else {
            animator.SetFloat("Speed", 0);
        }
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
