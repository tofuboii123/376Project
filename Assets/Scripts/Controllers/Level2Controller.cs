using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Level2Controller : MonoBehaviour
{
    public Image img;

    public GameObject xInteractText;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LevelStart());
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
            "You can combine two items by dragging one over the other using your mouse.",
            "Combine the two halves of a key on the floor nearby."}, new int[] {
            Face.None,
            Face.None
        });

        while (MessageController.showMessage > 0) {
            yield return null;
        }

        PlayerController.CanMove = true;
        xInteractText.SetActive(true);
    }

     public IEnumerator LevelEnd()
    {
        PlayerController.inCutscene= true;
        MessageController.ShowMessage("The door is unlocked..I know this is what\nVictoria wants. I just need to bury her daughter\nthen I go home..", Face.Thinking);
        while (MessageController.showMessage > 0)
        {
            yield return null;
        }
        for (float i = 0; i <= 3; i += Time.deltaTime)
        {
            // set color with i as alpha
            img.color = new Color(0, 0, 0, i);
            yield return null;
        }
        PlayerController.inCutscene= false;

        SceneManager.LoadScene("TutorialScene");
    }
}
