using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InteractableTree : Interactable
{
    public Image img;

    public override void OnInteraction() {
        TutorialController tutorialController = GameObject.Find("TutorialController").GetComponent<TutorialController>();
        StartCoroutine(tutorialController.LevelEnd());
    }
}
