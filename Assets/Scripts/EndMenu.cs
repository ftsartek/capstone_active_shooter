using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndMenu : MonoBehaviour

{
    public GameObject player;

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
        gameObject.SetActive(true);
        TMPro.TextMeshProUGUI message = GetComponentInChildren(typeof(TMPro.TextMeshProUGUI)) as TMPro.TextMeshProUGUI;
        
        if (died == true)
        {
            message.text = "You died";
        } else
        {
            message.text = "You escaped";
        }

        Cursor.lockState = CursorLockMode.None;

        // Do we need to stop the attacker from attacking us ?
        //Time.timeScale = 0;

        CharacterController cc = player.GetComponent(typeof(CharacterController)) as CharacterController;
        cc.enabled = false;
    }

    public void tryAgain()
    {
        // call the game start again?
    }

    public void quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        //Application.Quit();
    }
}
