using System.Collections;
using System.Collections.Generic;
using System.Threading;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using System;
using UnityEngine.UIElements;
using System.Timers;


public class ShootingTests {
    private float timeout = 30f;
    private float closeOffset = 0.1f;

    [UnityTest] public IEnumerator PlayerTargeted() {
        SceneManager.LoadScene("Scenes/Testing/ShooterTestScene", LoadSceneMode.Single);
        yield return new WaitForSeconds(4f); //wait for scene to initialise

        ScenarioControl scenario = GameObject.Find("PlayerBody").GetComponent<ScenarioControl>();
        Assert.That(scenario != null);

        float timer = 0f;

        while (!scenario.dead && timer < timeout) {
            yield return new WaitForEndOfFrame();
            timer += Time.deltaTime;
        }

        Assert.That(scenario.dead);
        LogAssert.ignoreFailingMessages = true;
    }

    [UnityTest]
    public IEnumerator ShooterExits() {
        //This is primarily to test that the shooter moves to the exit after the specified time
        //Not exit collision, which is just incidental

        SceneManager.LoadScene("Scenes/Testing/ShooterTestScene", LoadSceneMode.Single);
        yield return new WaitForSeconds(2f); //wait for scene to initialise

        //move player out of way so it doesn't mess things up
        Debug.Log("Moving player");
        GameObject player = GameObject.Find("Player");
        player.transform.position = new Vector3(-100, -500, -100);

        GameObject shooter = GameObject.Find("Shooter");
        Vector3 exit = GameObject.Find("MainExit").transform.position;
        Assert.That(shooter != null && exit != null);

        shooter.GetComponent<ShooterAI>().exitTime = 5f;
        float timer = 0f;

        while (Time.timeScale != 0 && timer < timeout) {
            yield return new WaitForEndOfFrame();
            timer += Time.deltaTime;
        }

        Assert.That(Time.timeScale == 0);
        LogAssert.ignoreFailingMessages = true;
    }

    private bool isClose(Vector3 pos1, Vector3 pos2) {
        return Vector3.Distance(pos1, pos2) <= closeOffset;
    }
}
