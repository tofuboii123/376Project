using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;
using TMPro;

public class PlayerController : MonoBehaviour
{
    // Getter and setter
    public static bool CanMove { get; set; }
    public GameObject MrInvisible;
    [SerializeField]
    float speed = 5.0f;

    [SerializeField]
    bool inPast = false;
    private bool isSafeSpot;
    private int numOfTries;

    Vector2 telePosition;
    [SerializeField]
    float travel = 150.0f; // Distance of 2nd timeline in y

    [SerializeField]
    Text timeIndicator;

    public TextMeshProUGUI interactTextObject;

    private Vector2 movement;

    public Animator animator;

    private Rigidbody rb;
    public PostProcessVolume volume;
    private Bloom bloom;
    private LensDistortion lensDistortion;
    private Grain grain;
    private DepthOfField depthOfField;
    private ColorGrading colorGrading;
    private ChromaticAberration chromaticAberration;

    void Start()
    {
        volume.profile.TryGetSettings(out bloom);
        volume.profile.TryGetSettings(out lensDistortion);
        volume.profile.TryGetSettings(out grain);
        volume.profile.TryGetSettings(out depthOfField);
        volume.profile.TryGetSettings(out colorGrading);
        volume.profile.TryGetSettings(out chromaticAberration);

        isSafeSpot = true;
        numOfTries = 0;

        CanMove = true;
        timeIndicator.text = "Present";
    }

    // Update is called once per frame
    void Update()
    {
        if (CanMove)
        {
            // Time travel.
            if (Input.GetButtonDown("TimeShift"))
                TimeShift();
        }
        else
        {
            animator.SetFloat("Speed", 0);
        }
    }

    void FixedUpdate()
    {
        if (CanMove)
        {
            MovePlayer();
        }
    }

    void MovePlayer()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        if (movement != Vector2.zero)
        {
            animator.SetFloat("Horizontal", movement.x);
            animator.SetFloat("Vertical", movement.y);
        }

        animator.SetFloat("Speed", movement.sqrMagnitude);

        this.transform.Translate(movement.normalized * speed * Time.deltaTime);
    }

    // Go from past to present and vice-versa
    void TimeShift()
    {
        // Boolean switch
        inPast = !inPast;

        // HUD element
        // TODO change for cool animation
        Clock.TimeTravel();
        timeIndicator.text = inPast ? "Past" : "Present";

        StartCoroutine(StartPostProcessingEffect());
    }

    // Time shift animation
    IEnumerator StartPostProcessingEffect() {
        CanMove = false;
        float timer;

        timer = 0.0f;
        while (timer < 0.5f) {
            timer += Time.deltaTime;

            bloom.intensity.value = Mathf.Lerp(0.0f, 25.0f, timer / 0.5f);
            lensDistortion.intensity.value = Mathf.Lerp(0.0f, 20.0f, timer / 0.5f);
            depthOfField.focalLength.value = Mathf.Lerp(0.0f, 67.0f, timer / 0.5f);
            if (inPast) {
                chromaticAberration.intensity.value = Mathf.Lerp(0.0f, 0.5f, timer / 0.5f);
            }

            yield return null;
        }
        float x = MrInvisible.transform.position.x;
        float y = MrInvisible.transform.position.y;

        MrInvisible.transform.position = new Vector2(-10, -20);

        //this.transform.position = checkTeleportPosition();
        this.transform.position = new Vector2(x, y);

        grain.intensity.value = (inPast ? 1 : 0);

        if (inPast)
        {
            colorGrading.active = true;
            colorGrading.mixerBlueOutGreenIn.value = 50;
            colorGrading.mixerBlueOutBlueIn.value = 50;
            colorGrading.mixerBlueOutRedIn.value = 50;
        }
        else
        {
            colorGrading.active = false;
            colorGrading.mixerBlueOutGreenIn.value = 0;
            colorGrading.mixerBlueOutBlueIn.value = 0;
            colorGrading.mixerBlueOutRedIn.value = 0;
        }

        timer = 0.0f;
        while (timer < 0.5f) {
            timer += Time.deltaTime;

            bloom.intensity.value = Mathf.Lerp(25.0f, 0.0f, timer / 0.5f);
            lensDistortion.intensity.value = Mathf.Lerp(20.0f, 0.0f, timer / 0.5f);
            depthOfField.focalLength.value = Mathf.Lerp(67.0f, 0.0f, timer / 0.5f);
            if (!inPast) {
                chromaticAberration.intensity.value = Mathf.Lerp(0.5f, 0.0f, timer / 0.5f);
            }

            yield return null;
        }

        CanMove = true;
    }

    //just a major WIP, please ignore
    Vector3 checkTeleportPosition()
    {
        telePosition = this.transform.position + new Vector3(0, (inPast ? travel : -travel), 0);
        RaycastHit2D hit = Physics2D.Raycast(telePosition, Vector2.up);

        if (hit.collider != null && hit.collider.bounds.Contains(new Vector3(telePosition.x, telePosition.y, hit.transform.position.z)))
        {
            isSafeSpot = false;
            /*
            RaycastHit2D hit = Physics2D.Raycast(MrInvisible.transform.position, Vector2.up);

            if  (hit.collider != null && hit.collider.bounds.Contains(new Vector3(MrInvisible.transform.position.x, MrInvisible.transform.position.y, hit.transform.position.z)) && (hit.collider.gameObject.layer != 10))
            {
                Debug.Log("up");
                //up
                MrInvisible.transform.position = new Vector2(hit.collider.bounds.center.x, hit.collider.bounds.center.y + (hit.collider.bounds.size.y/2));
                RaycastHit2D hit2 = Physics2D.Raycast(MrInvisible.transform.position, Vector2.up);

                //If up is occupied
                if (hit2.collider != null && hit2.collider.bounds.Contains(new Vector3(MrInvisible.transform.position.x + MrInvisible.GetComponent<BoxCollider2D>().offset.x, MrInvisible.transform.position.y + MrInvisible.GetComponent<BoxCollider2D>().offset.y, hit.transform.position.z)) && (hit.collider.gameObject.layer != 10))
                {
                    Debug.Log("down");

                    //down
                    MrInvisible.transform.position = new Vector2(hit.collider.bounds.center.x, hit.collider.bounds.center.y - (hit.collider.bounds.size.y / 2) );
                    hit2 = Physics2D.Raycast(MrInvisible.transform.position, Vector2.down);

                    //if down is occupied
                    if (hit2.collider != null && hit2.collider.bounds.Contains(new Vector3(MrInvisible.transform.position.x + MrInvisible.GetComponent<BoxCollider2D>().offset.x, MrInvisible.transform.position.y + MrInvisible.GetComponent<BoxCollider2D>().offset.y, hit.transform.position.z)) && (hit.collider.gameObject.layer != 10))
                    {
                        Debug.Log("left");

                        //left
                        MrInvisible.transform.position = new Vector2(hit.collider.bounds.center.x - (hit.collider.bounds.size.x / 2) - (this.gameObject.GetComponent<BoxCollider2D>().size.x / 1.8f), hit.collider.bounds.center.y);
                        hit2 = Physics2D.Raycast(MrInvisible.transform.position, Vector2.up);
                        if (hit2.collider != null && hit2.collider.bounds.Contains(new Vector3(MrInvisible.transform.position.x, MrInvisible.transform.position.y, hit2.transform.position.z)) && (hit.collider.gameObject.layer != 10))
                        {
                            Debug.Log("right");

                            MrInvisible.transform.position = new Vector2(hit.collider.bounds.center.x + (hit.collider.bounds.size.x / 2) + (this.gameObject.GetComponent<BoxCollider2D>().size.x / 1.8f), hit.collider.bounds.center.y);
                        }
                    }
                }
            }*/
        }

        return telePosition;
    }

}
