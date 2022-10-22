using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using ShooterClass = ShooterAI;

public class Exit : MonoBehaviour
{
    // end menu canvas
    EndMenuScript endMenu;

    // literally just here for the trigger unit test
    public string colliding = "";

    void Start()
    {
        GameObject endFrame = GameObject.Find("EndFrame");
        endMenu = endFrame.GetComponent<EndMenuScript>();
    }

    private void OnTriggerEnter(Collider other)
    {
        colliding = other.tag;

        // deal with escaping player
        if (other.tag == "Player")
        {
            Debug.Log("Exit triggered by player");
            //end game
            endMenu.ToggleEndMenu(false);
        }

        // deal with escaping NPCs
        if (other.tag == "NPC")
        {
            // make NPC disappear
        }

        if (other.tag == "Shooter")
        {
            Debug.Log("Shooter Exiting");
            // check the shooter wants to exit
            if (other.transform.parent.gameObject.GetComponent<ShooterAI>().state == State.Exiting)
            {
                Debug.Log("Exit triggered by shooter");
                //end game
                endMenu.ToggleEndMenu(false);
            }

        }
    }
}
