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
        //Debug.Log("here1");
        SceneManager.LoadScene("Scenes/Testing/TriggerTestScene", LoadSceneMode.Single);
        yield return new WaitForSeconds(4f); //wait for scene to initialise

        //Debug.Log("here2");
        GameObject player = GameObject.Find("Player");
        var mainExit = GameObject.Find("MainExit");
        //var secondaryExit = GameObject.Find("SecondaryExit");
        //Debug.Log("here3");
        //Debug.Log(player.transform.position);
        //Debug.Log(mainExit.transform.position);
        //Debug.Log("au milieu");
        //Vector3 pos = mainExit.transform.position;
        //player.transform.position = pos;
        //Debug.Log(player.tag);
        player.transform.position += new Vector3(-3.5f, 0f, 0f);
        //Debug.Log(player.transform.position);
        //Debug.Log(mainExit.transform.position);
        //Debug.Log("here4");
        yield return new WaitForSeconds(4f);
        //Debug.Log("here5");

        while (mainExit.GetComponent<Exit>().colliding == "")
        {
            yield return new WaitForEndOfFrame();
        }
        //Debug.Log(mainExit.GetComponent<Exit>().colliding);
        //Debug.Log(player.tag);
        Assert.IsTrue(mainExit.GetComponent<Exit>().colliding == player.tag);

        //player.transform.position = secondaryExit.transform.position;
        //yield return new WaitForSeconds(.5f);
        //Assert.IsTrue(secondaryExit.GetComponent<Collider>().tag == player.tag);

        //Assert.That(scenario.dead);
        // Use the Assert class to test conditions.
        // Use yield to skip a frame.
        //yield return null;
    }
}
