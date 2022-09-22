using System.Collections;
using System.Collections.Generic;
using System.Threading;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using System;
using UnityEngine.UIElements;


public class ShootingTests {
    private float timeout = 30f;

    [UnityTest] public IEnumerator PlayerTargeted() {
        SceneManager.LoadScene("Scenes/Testing/TestScene", LoadSceneMode.Single);
        yield return new WaitForSeconds(4f); //wait for scene to initialise

        ScenarioControl scenario = GameObject.Find("PlayerBody").GetComponent<ScenarioControl>();
        Assert.That(scenario != null && scenario.debug);

        float timer = 0f;

        while (!scenario.dead && timer < timeout) {
            yield return new WaitForEndOfFrame();
            timer += Time.deltaTime;
        }

        Assert.That(scenario.dead);
        LogAssert.ignoreFailingMessages = true;
    }
}
