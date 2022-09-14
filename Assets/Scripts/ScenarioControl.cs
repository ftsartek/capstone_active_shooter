using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenarioControl : MonoBehaviour {
    private static string shot = "You have been shot!";

    private static int maxBullets = 4;
    private int bulletHits = 0;
    private bool killed = false;

    void OnCollisionEnter(Collision collision) {
        if (!collision.gameObject.name.StartsWith("Bullet")) return;

        bulletHits += 1;

        if (bulletHits < maxBullets) {
            Debug.Log("Hit!");
        }
        else {
            killed = true;
            Time.timeScale = 0;
        }
    }

    void OnGUI() {
        if (killed) {
            GUI.Label(new Rect(Screen.width / 2 - 50, Screen.height / 2 - 10, 500, 500), shot);
        }
    }
}
