using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class CreditsController : MonoBehaviour {
    public Image img;
    public RectTransform creditsTextObjectTransform;
    public TextMeshProUGUI thanksText;

    // Start is called before the first frame update
    void Start() {
        creditsTextObjectTransform.anchorMin = new Vector2(0.5f, -0.05f);
        creditsTextObjectTransform.anchorMax = new Vector2(0.5f, -0.05f);

        //StartCoroutine(FadeOut());
        StartCoroutine(StartCredits());
    }

    IEnumerator StartCredits() {
        float timer;
        Color thanksTextColor;

        yield return new WaitForSeconds(0.5f);

        timer = 0.0f;
        while (timer < 15.0f) {
            timer += Time.deltaTime;

            creditsTextObjectTransform.anchorMin = new Vector2(0.5f, Mathf.Lerp(-0.05f, 1.35f, timer / 15.0f));
            creditsTextObjectTransform.anchorMax = new Vector2(0.5f, Mathf.Lerp(-0.05f, 1.35f, timer / 15.0f));

            yield return null;
        }

        thanksTextColor = thanksText.color;

        timer = 0.0f;
        while (timer < 2.0f) {
            timer += Time.deltaTime;

            thanksText.color = Color.Lerp(thanksTextColor, new Color(255, 255, 255, 1), timer / 2.0f);

            yield return null;
        }

        yield return new WaitForSeconds(2.0f);

        thanksTextColor = thanksText.color;

        timer = 0.0f;
        while (timer < 2.0f) {
            timer += Time.deltaTime;

            thanksText.color = Color.Lerp(thanksTextColor, new Color(255, 255, 255, 0), timer / 2.0f);

            yield return null;
        }

        yield return new WaitForSeconds(1.0f);

        SceneManager.LoadScene("MainMenu");
    }
}