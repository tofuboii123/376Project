using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TutorialController : MonoBehaviour
{
    public Image img;
    public static bool firstAppreance = true;
    public GameObject grave;
    public GameObject plant;
    public GameObject tree;
    public Animator animator;
    public GameObject Player;

    // Start is called before the first frame update
    void Start()
    {
        if(firstAppreance){
        StartCoroutine(LevelStart());
        } else{
            StartCoroutine(FadeOut());
            Inventory inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
            inventory.AddItem(GameObject.Find("The Heart"));
            Player.transform.position = new Vector3(0.1f,-5.19f,Player.transform.position.z);
            plant.SetActive(false);
            tree.GetComponent<InteractableTree>().interactTextString = "Bury Sabrina";
        }
    }

    IEnumerator FadeOut()
    {
        // loop over 1 second backwards
        for (float i = 2; i >= 0; i -= Time.deltaTime)
        {
            // set color with i as alpha
            img.color = new Color(0, 0, 0, i);
            yield return null;
        }
    }

    IEnumerator LevelStart()
    {
        // loop over 1 second backwards
        for (float i = 2; i >= 0; i -= Time.deltaTime)
        {
            // set color with i as alpha
            img.color = new Color(0, 0, 0, i);
            yield return null;
        }

        MessageController.ShowMessage(new string[] {
            "I need to get inside the mansion. Something awaits me in the basement.",
            "Use WASD to move.\nPress X to interact with objects.\nPress O to open your inventory." }, new int[] {
            Face.Thinking,
            Face.None
        });
        firstAppreance = false;
    }

    public IEnumerator LevelEnd()
    {
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
        
        for (float i = 0; i <= 3; i += Time.deltaTime)
        {
            // set color with i as alpha
            img.color = new Color(0, 0, 0, i);
            yield return null;
        }
        SceneManager.LoadScene("Level1");
    }

    
    public IEnumerator GameEnd()
    {
        PlayerController.inCutscene = true;
        MessageController.ShowMessage(new string[] {
            "I can bury her right next to this tree.\nAfter all these years..Her soul can finally\nrest in peace.",
        }, new int[] {
           
            Face.Thinking
        });

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
        });

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

        SceneManager.LoadScene("MainMenu");
    }
}
