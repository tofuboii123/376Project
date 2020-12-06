using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InteractableTree : Interactable {
    public Image img;

    private bool isInteractingWithTree;

    void Start() {
        isInteractingWithTree = false;
    }

    public override void OnInteraction() {
        if (isInteractingWithTree) {
            return;
        }

        isInteractingWithTree = true;

        if (!inventory.ContainsItem(220)) {
            TutorialController tutorialController = GameObject.Find("TutorialController").GetComponent<TutorialController>();
            StartCoroutine(tutorialController.LevelEnd());
        } else {
            inventory.DiscardItem(220);
            TutorialController tutorialController = GameObject.Find("TutorialController").GetComponent<TutorialController>();
            StartCoroutine(tutorialController.GameEnd());
        }
    }
}
