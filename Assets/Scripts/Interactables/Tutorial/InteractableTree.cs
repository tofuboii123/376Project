using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InteractableTree : Interactable {
    public Image img;

    private bool isClimbingTree;

    void Start() {
        isClimbingTree = false;
    }

    public override void OnInteraction() {
        if (isClimbingTree) {
            return;
        }

        isClimbingTree = true;
        if (TutorialController.firstAppreance) {
            TutorialController tutorialController = GameObject.Find("TutorialController").GetComponent<TutorialController>();
            StartCoroutine(tutorialController.LevelEnd());
        } else {
            inventory.DiscardItem(220);
            TutorialController tutorialController = GameObject.Find("TutorialController").GetComponent<TutorialController>();
            StartCoroutine(tutorialController.GameEnd());
        }
    }
}
