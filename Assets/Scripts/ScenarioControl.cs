using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenarioControl : MonoBehaviour {
    public static int maxBullets = 2;

    private int bulletHits = 0;
    private float timer;

    private EndMenu endMenu;

    void OnEnable() {
        GameObject endFrame = GameObject.Find("EndFrame");
        endMenu = endFrame.GetComponent<EndMenu>();


        bulletHits = 0;
    }

    void OnCollisionEnter(Collision collision) {
        if (!collision.gameObject.name.StartsWith("Bullet")) return;

        bulletHits += 1;

        if (bulletHits < maxBullets) {
            Debug.Log("Hit!");
        }
        else {
            endMenu.ToggleEndMenu(true);
        }
    }
}