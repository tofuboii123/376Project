using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TutorialController : MonoBehaviour
{
    public Image img;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LevelStart());
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
}
