using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Level2Controller : MonoBehaviour
{
    public Image img;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LevelStart());
    }

    IEnumerator LevelStart()
    {
        PlayerController.CanMove = false;

        // loop over 1 second backwards
        for (float i = 2; i >= 0; i -= Time.deltaTime)
        {
            // set color with i as alpha
            img.color = new Color(0, 0, 0, i);
            yield return null;
        }

        Inventory inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
        if (PlayerController.hasOddKey)
        {
            inventory.AddItem(GameObject.Find("Oddly Shaped Key"));
        }

        MessageController.ShowMessage(new string[] {
            "You can combine two items by dragging one over the other using your mouse.",
            "Combine the two halves of a key on the floor nearby."}, new int[] {
            Face.None,
            Face.None
        });

        PlayerController.CanMove = true;
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
