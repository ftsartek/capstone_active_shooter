using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenarioControl : MonoBehaviour {
    public static int maxBullets = 3;
    public static int maxAttacks = 4;
    [HideInInspector] public bool dead = false;

    private int bulletHits = 0;
    private int attacksMade = 0;

    private EndMenuScript endMenu;

    private void OnEnable() {
        GameObject endFrame = GameObject.Find("EndFrame");
        endMenu = endFrame.GetComponent<EndMenuScript>();

        bulletHits = 0;

        // get shooter object
        GameObject shooter = GameObject.FindGameObjectWithTag("Shooter");

        // get door the shooter will enter from 
        GameObject[] EntranceList = GameObject.FindGameObjectsWithTag("MainExit");
        GameObject entrance = EntranceList[Random.Range(0, EntranceList.Length)];

        // teleport shooter 
        shooter.SetActive(false);
        shooter.transform.position = entrance.transform.position;
        shooter.SetActive(true);
  
    }

    private void OnCollisionEnter(Collision collision) {
        if (!collision.gameObject.name.StartsWith("Bullet")) return;

        bulletHits += 1;

        if (bulletHits < maxBullets) {
            Debug.Log("Hit!");
        }
        else {
            Debug.Log("Killed!");
            dead = true;

            endMenu.ToggleEndMenu(true);
        }
    }

    public void Attack() {
        attacksMade += 1;

        if (attacksMade < maxAttacks) {
            Debug.Log("Attacked!");
        }
        else {
            Debug.Log("Won!");
            endMenu.ToggleEndMenu(false);
        }
    }
}
