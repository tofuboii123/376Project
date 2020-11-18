using UnityEngine.SceneManagement;

public class InteractableTree : Interactable
{
    public override void OnInteraction() {
        //SceneManager.LoadScene(1);
        print("Load level 1");
    }
}
