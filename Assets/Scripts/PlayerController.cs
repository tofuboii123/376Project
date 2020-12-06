using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;
using TMPro;

public class PlayerController : MonoBehaviour
{
    // Getter and setter
    public static bool CanMove { get; set; }
    public static bool inCutscene = false;

    public GameObject MrInvisible;
    [SerializeField]
    float speed = 5.0f;

    [SerializeField]
    public bool inPast = false;

    Vector2 telePosition;
    [SerializeField]
    float travel = 150.0f; // Distance of 2nd timeline in y

    [SerializeField]
    Text timeIndicator;

    public TextMeshProUGUI interactTextObject;

    private Vector2 movement;

    public Animator animator;

    public static bool isTravelling;
    public PostProcessVolume volume;

    private Bloom bloom;
    private LensDistortion lensDistortion;
    private Grain grain;
    private DepthOfField depthOfField;
    private ColorGrading colorGrading;
    private ChromaticAberration chromaticAberration;

    private AudioManager audioManager;

    private float canFullScreenInventory;
    private float canPlayWalkSoundAgain;

    void Start()
    {
        volume.profile.TryGetSettings(out bloom);
        volume.profile.TryGetSettings(out lensDistortion);
        volume.profile.TryGetSettings(out grain);
        volume.profile.TryGetSettings(out depthOfField);
        volume.profile.TryGetSettings(out colorGrading);
        volume.profile.TryGetSettings(out chromaticAberration);

        bloom.active = false;
        lensDistortion.active = false;
        depthOfField.active = false;
        colorGrading.active = false;
        chromaticAberration.active = false;

        inCutscene = false;
        isTravelling = false;
        timeIndicator.text = "Present";

        audioManager = FindObjectOfType<AudioManager>();
        canPlayWalkSoundAgain = -1;
    }

    // Update is called once per frame
    void Update()
    {
        // Full screen inventory
        if (Input.GetButtonDown("FullScreenInventory")) {
            if (FullScreenInventory.inHelpScreen) {
                return;
            }

            if (Time.time < canFullScreenInventory) {
                return;
            }

            if (isTravelling) {
                return;
            }

            canFullScreenInventory = Time.time + 0.5f;

            if (FullScreenInventory.inMenu) {
                CanMove = true;
                FullScreenInventory.exitFullScreenInventory();
            } else {
                CanMove = false;
                FullScreenInventory.startFullScreenInventory();
            }
        }

        if (CanMove) {
            // Time travel.
            if (Input.GetButtonDown("TimeShift")) {
                if (!Cutscene5_Finale.goodEndingTriggered) {
                    TimeShift();
                } else {
                    StartCoroutine(ClockNotWorking());
                }
            }
        } else {
            animator.SetFloat("Speed", 0);
        }
    }

    void FixedUpdate()
    {
        if (CanMove && !inCutscene)
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

            if (Time.time >= canPlayWalkSoundAgain) {
                canPlayWalkSoundAgain = Time.time + 0.42f;

                GetAudioManager();
                string walkingSound = (Random.Range(0.0f, 1.0f) < 0.5f ? "Walking 1" : "Walking 2");
                audioManager.PlayIfNotPlaying(walkingSound);
            }
        }

        animator.SetFloat("Speed", movement.sqrMagnitude);

        this.transform.Translate(movement.normalized * speed * Time.deltaTime);
    }

    // Go from past to present and vice-versa
    public void TimeShift() {
        CanMove = false;
        isTravelling = true;

        // Boolean switch
        inPast = !inPast;

        GetAudioManager();
        string soundToPlay;
        if (inPast) {
            soundToPlay = "Time Travel To Past";
            audioManager.PlayFadeIn("In Past", 1.0f);
        } else {
            soundToPlay = "Time Travel To Present";
            audioManager.StopFadeOut("In Past", 1.0f);
        }

        audioManager.Play(soundToPlay);
        
        Clock.TimeTravel();
        timeIndicator.text = inPast ? "Past" : "Present";

        StartCoroutine(StartPostProcessingEffect());
    }

    // Time shift animation
    public IEnumerator StartPostProcessingEffect() {
        float timer;

        bloom.active = true;
        lensDistortion.active = true;
        depthOfField.active = true;
        chromaticAberration.active = true;

        MrInvisible.transform.position = new Vector2(this.transform.position.x, inPast ? this.transform.position.y + travel : this.transform.position.y - travel);

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
        MrInvisible.transform.position = new Vector2(-1000, -1000);

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

        bloom.active = false;
        lensDistortion.active = false;
        depthOfField.active = false;
        if (!inPast) {
            chromaticAberration.active = false;
        }

        CanMove = true;

        //is set to true from whichever script that cares if the player is travelling
        isTravelling = false;
    }
    public IEnumerator ClockNotWorking() {

        CanMove = false;
        MessageController.ShowMessage(new string[] { "???\nThe watch seems to have stopped working.." });
        while (MessageController.showMessage > 0) {
            yield return null;
        }
        CanMove = true;
    }

    private void GetAudioManager() {
        if (audioManager == null) {
            audioManager = FindObjectOfType<AudioManager>();
        }
    }
}
