using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cutscene_Present : MonoBehaviour
{
    public static bool inPresentTriggered;
    // Start is called before the first frame update
    void Start()
    {
        inPresentTriggered = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.CompareTo("Player") == 0)
        {
            if (!inPresentTriggered){
            PlayerController.isTravelling = true;

            inPresentTriggered = true;
            collision.gameObject.GetComponent<PlayerController>().TimeShift();

        }
        }
    }
}
