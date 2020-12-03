using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractableOfficeDoorPresent : Interactable
{
    public float DestX;
    public float DestY;
    public Image img;

    public static bool rustIsApplied = false;
    private bool firstMessageShown = false;

    private AudioManager audioManager;

    public override void OnInteraction()
    {
        if (rustIsApplied)
        {
            if (!firstMessageShown)
            {
                MessageController.ShowMessage("The rust has weakened the door lock. I broke it easily. I can enter the room now.", Face.Happy);
                firstMessageShown = true;
            }
            else
            {
                StartCoroutine(Transition());
            }
        }
        else
        {
            if (inventory.ContainsSelectedItem(13))
            {
                MessageController.ShowMessage("I don't think now is the right time to use this.", Face.Thinking);
            }
            else
            {
                MessageController.ShowMessage("The lock is jammed. A key won't work here. I need to find a different way to unlock this door.", Face.Thinking);
            }
            
        }
    }

    IEnumerator Transition()
    {

        PlayerController.CanMove = false;

        GetAudioManager();
        audioManager.Play("Open Door");

        for (float i = 0; i <= 1; i += Time.deltaTime * 2)
        {
            // set color with i as alpha
            img.color = new Color(0, 0, 0, i);
            yield return null;
        }

        player.transform.position = new Vector3(DestX, DestY, player.transform.position.z);

        for (float i = 1; i >= 0; i -= Time.deltaTime * 2)
        {
            // set color with i as alpha
            img.color = new Color(0, 0, 0, i);
            yield return null;
        }
        img.color = new Color(0, 0, 0, 0);
        PlayerController.CanMove = true;

    }

    private void GetAudioManager() {
        if (audioManager == null) {
            audioManager = FindObjectOfType<AudioManager>();
        }
    }
}
