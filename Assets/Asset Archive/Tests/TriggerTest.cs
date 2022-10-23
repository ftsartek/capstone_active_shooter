using System.Collections;
using System.Collections.Generic;
using System.Threading;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using System;
using UnityEngine.UIElements;

public class TriggerTest
{
    // A Test behaves as an ordinary method
    [Test]
    public void TriggerTestSimplePasses()
    {
        // Use the Assert class to test conditions
    }

    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator TriggerTestWithEnumeratorPasses()
    {
        SceneManager.LoadScene("Scenes/Testing/TriggerTestScene", LoadSceneMode.Single);
        yield return new WaitForSeconds(4f); //wait for scene to initialise

        GameObject player = GameObject.Find("Player");
        var mainExit = GameObject.Find("MainExit");
        
        player.transform.position += new Vector3(-3.5f, 0f, 0f);
        
        yield return new WaitForSeconds(4f);

        while (mainExit.GetComponent<Exit>().colliding == "")
        {
            yield return new WaitForEndOfFrame();
        }
        
        Assert.IsTrue(mainExit.GetComponent<Exit>().colliding == player.tag);

    }
}
