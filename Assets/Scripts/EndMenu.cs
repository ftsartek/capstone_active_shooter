using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndMenu : MonoBehaviour

{
    // not sure if this is needed, maybe 
    //public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ToggleEndMenu(bool died)
    {
        Debug.Log(gameObject);
        // make menu appear 
        gameObject.SetActive(true);

        // get text object
        TMPro.TextMeshProUGUI message = GetComponentInChildren(typeof(TMPro.TextMeshProUGUI)) as TMPro.TextMeshProUGUI;
        
        // change message depending on win/lose 
        if (died == true)
        {
            message.text = "You died";
        } else
        {
            message.text = "You escaped";
        }

        // makes cursor appear
        Cursor.lockState = CursorLockMode.None;

        // stops all interactions while in the end menu, set back to 1 in tryAgain()
        Time.timeScale = 0;
    }

    // for now this function gets called upon button press but does nothing
    public void tryAgain()
    {
        Time.timeScale = 1;
        // call to start the game again?
    }

    // quit gain upon button press
    public void quit()
    {
        //works for both editor and when built
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }
}
