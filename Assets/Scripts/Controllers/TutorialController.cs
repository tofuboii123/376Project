using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TutorialController : MonoBehaviour
{
    public Image img;
    public GameObject grave;
    public GameObject plant;
    public GameObject tree;
    public Animator animator;
    public GameObject Player;

    public GameObject xInteractText;

    private AudioManager audioManager;

    // Start is called before the first frame update
    void Start()
    {
        if (!Player.GetComponent<Inventory>().ContainsItem(220)) {
            StartCoroutine(LevelStart());
        } else{
            StartCoroutine(FadeOut());

            Player.transform.position = new Vector3(0.1f,-5.19f,Player.transform.position.z);
            plant.SetActive(false);

            tree.GetComponent<InteractableTree>().interactTextString = "Bury Sabrina";
        }
    }

    IEnumerator FadeOut()
    {
        PlayerController.CanMove = false;
        // loop over 1 second backwards
        for (float i = 2; i >= 0; i -= Time.deltaTime)
        {
            // set color with i as alpha
            img.color = new Color(0, 0, 0, i);
            yield return null;
        }

        PlayerController.CanMove = true;
    }

    IEnumerator LevelStart()
    {
        PlayerController.CanMove = false;
        xInteractText.SetActive(false);
        // loop over 1 second backwards
        for (float i = 2; i >= 0; i -= Time.deltaTime)
        {
            // set color with i as alpha
            img.color = new Color(0, 0, 0, i);
            yield return null;
        }

        MessageController.ShowMessage(new string[] {
            "I need to get inside the mansion. Something awaits me in the basement.",
            "Use WASD to move.\n\nPress F to interact with objects.\nPress R or ESC to open your inventory.\nPress space to travel between time periods.\nPress Q/E to cycle through your inventory" }, new int[] {
            Face.Thinking,
            Face.None,
        });

        while (MessageController.showMessage > 0) {
            yield return null;
        }

        PlayerController.CanMove = true;
        xInteractText.SetActive(true);
    }

    public IEnumerator LevelEnd() {
        MessageController.ShowMessage(new string[] {
            "I can climb the tree into the open window.",
            "Time to see what's in the mansion..."
        }, new int[] {
            Face.Happy,
            Face.Thinking
        });

        while (MessageController.showMessage > 0) {
            yield return null;
        }

        GetAudioManager();
        audioManager.PlayFadeIn("Climb Tree", 0.5f);

        for (float i = 0; i <= 3; i += Time.deltaTime) {
            // set color with i as alpha
            img.color = new Color(0, 0, 0, i);
            yield return null;
        }

        audioManager.StopFadeOut("Climb Tree", 1.5f);

        SceneManager.LoadScene("Level1");
    }
    
    public IEnumerator GameEnd()
    {
        PlayerController.inCutscene = true;
        MessageController.ShowMessage(new string[] {
            "I can bury her right next to this tree.\nAfter all these years.. Her soul can finally\nrest in peace.",
        }, new int[] {
           
            Face.Thinking
        }, false);

        while (MessageController.showMessage > 0) {
            yield return null;
        }
     
        yield return new WaitForSeconds(1);
        grave.transform.position = new Vector3(Player.transform.position.x+1.5f,grave.transform.position.y,grave.transform.position.z);
        grave.SetActive(true);
        yield return new WaitForSeconds(2);

        MessageController.ShowMessage(new string[] {
            "Victoria:\nThank you Ashton..\nI shall leave your body now.",
        }, new int[] {
           
            Face.Thinking
        }, false);

          while (MessageController.showMessage > 0) {
            yield return null;
        }
        yield return new WaitForSeconds(0.5f);

        for (float i = 0; i <= 3; i += Time.deltaTime)
        {
            // set color with i as alpha
            img.color = new Color(0, 0, 0, i);
            yield return null;
        }

        PlayerController.inCutscene = false;
        Cutscene5_Finale.goodEndingTriggered = false;
        SceneManager.LoadScene("MainMenu");
    }

    private void GetAudioManager() {
        if (audioManager == null) {
            audioManager = FindObjectOfType<AudioManager>();
        }
    }
}
