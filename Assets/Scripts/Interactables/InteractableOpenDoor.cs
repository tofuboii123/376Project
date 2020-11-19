using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractableOpenDoor : Interactable
{
    public float DestX;
    public float DestY;
    public GameObject player;
    public Image img;

    public override void OnInteraction()
    {
        StartCoroutine(Transition());
    }

    IEnumerator Transition()
    {

        PlayerController.CanMove = false;

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

}