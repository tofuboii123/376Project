using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

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

    public PostProcessVolume volume;
    private Bloom bloom;
    private LensDistortion lensDistortion;
    private Grain grain;
    private DepthOfField depthOfField;
    private ColorGrading colorGrading;
    private ChromaticAberration chromaticAberration;

    void Start() {
        volume.profile.TryGetSettings(out bloom);
        volume.profile.TryGetSettings(out lensDistortion);
        volume.profile.TryGetSettings(out grain);
        volume.profile.TryGetSettings(out depthOfField);
        volume.profile.TryGetSettings(out colorGrading);
        volume.profile.TryGetSettings(out chromaticAberration);
    }

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
        inPast = !inPast;

        StartCoroutine(StartTimeShift());
    }

    IEnumerator StartTimeShift() {
        for (int i = 0; i < 100; i++) {
            bloom.intensity.value = i / 3;
            lensDistortion.intensity.value = i / 5;
            depthOfField.focalLength.value = i;
            chromaticAberration.intensity.value = i / 100;

            yield return new WaitForSeconds(0.005f);
        }

        this.transform.position += new Vector3(0, (inPast ? travel : -travel), 0); // Unity doesn't allow you to only modify the z value...
        grain.intensity.value = (inPast ? 1 : 0);

        if (inPast) {
            colorGrading.active = true;
            colorGrading.mixerBlueOutGreenIn.value = 50;
            colorGrading.mixerBlueOutBlueIn.value = 50;
            colorGrading.mixerBlueOutRedIn.value = 50;
        } else {
            colorGrading.active = false;
            colorGrading.mixerBlueOutGreenIn.value = 0;
            colorGrading.mixerBlueOutBlueIn.value = 0;
            colorGrading.mixerBlueOutRedIn.value = 0;
        }

        for (int i = 100; i >= 0; i--) {
            bloom.intensity.value = i / 3;
            lensDistortion.intensity.value = i / 5;
            depthOfField.focalLength.value = i;
            chromaticAberration.intensity.value = i / 100;

            yield return new WaitForSeconds(0.005f);
        }
    }
}