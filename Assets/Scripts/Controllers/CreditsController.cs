using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CreditsController : MonoBehaviour
{
    public Image img;
    public RectTransform creditsText;
    public Text thanksText;

    private float scrollSpeed = 50.0f;
    private float thanksFadeSpeed = 2.0f;
    private float creditsTextLimitY = 400.0f;
    private bool coroutineStarted = false;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(FadeOut());
    }

    // Update is called once per frame
    void Update()
    {
        if (creditsText.anchoredPosition.y > creditsTextLimitY && !coroutineStarted)
        {
            coroutineStarted = true;
            StartCoroutine(ThanksText());
        }
        else if(!coroutineStarted)
        {
            creditsText.anchoredPosition = new Vector2(creditsText.anchoredPosition.x, creditsText.anchoredPosition.y + scrollSpeed * Time.deltaTime);
        }
    }

    IEnumerator ThanksText()
    {
        while (thanksText.color.a < 0.95f)
        {
            thanksText.color = Color.Lerp(thanksText.color, new Color(255, 255, 255, 1), thanksFadeSpeed * Time.deltaTime);
            yield return null;
        }

        yield return new WaitForSeconds(2.0f);

        yield return StartCoroutine(FadeIn());

        SceneManager.LoadScene("MainMenu");
    }

    IEnumerator FadeIn()
    {
        for (float i = 0; i <= 3; i += Time.deltaTime)
        {
            // set color with i as alpha
            img.color = new Color(0, 0, 0, i);
            yield return null;
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
}
