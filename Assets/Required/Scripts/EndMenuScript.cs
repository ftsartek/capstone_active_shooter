using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndMenuScript : MonoBehaviour

{
    // not sure if this is needed, maybe 
    //public GameObject player;

    private GameObject endMenu;
    public bool debug = false;
    public GameObject player;


    // Start is called before the first frame update
    void Start()
    {
        endMenu = GameObject.Find("EndMenu");
        //GameObject endMenu = GetComponentInChildren(typeof(GameObject), true) as GameObject;
        endMenu.SetActive(false);
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ToggleEndMenu(bool died, string type)
    {
        Debug.Log(gameObject);
        // make menu appear 
        //EndMenu endMenu = GetComponentInChildren(typeof(EndMenu), true) as EndMenu;
        endMenu.SetActive(true);

        // get text object
        TMPro.TextMeshProUGUI message = GetComponentInChildren(typeof(TMPro.TextMeshProUGUI)) as TMPro.TextMeshProUGUI;
        
        // change message depending on win/lose 
        if (died == true)
        {
            message.text = "You died";
        } else
        {
            message.text = "You " + type;
        }

        // makes cursor appear
        Cursor.lockState = CursorLockMode.None;

        if (!debug) {
            // stops all interactions while in the end menu, set back to 1 in tryAgain()
            // This needs to be commented for the trigger test to pass
            Time.timeScale = 0;
        }
    }

    // for now this function gets called upon button press but does nothing
    public void tryAgain()
    {
        Debug.Log("Try again");
        Time.timeScale = 1;
        // call to start the game again?
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        player.SetActive(false);
        player.SetActive(true);
    }

    // quit gain upon button press
    public void quit()
    {
        Debug.Log("quit");
        //works for both editor and when built
        // if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        // endif
        Application.Quit();
    }
}
